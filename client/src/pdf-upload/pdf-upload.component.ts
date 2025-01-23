import { HttpClient } from '@angular/common/http';
import { Component, Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Message } from './message';
import { environment } from '../environments/environment.development';
import { Content } from './content';

@Component({
  selector: 'app-pdf-upload',
  templateUrl: './pdf-upload.component.html',
  styleUrls: ['./pdf-upload.component.css'],
})
@Injectable()
export class PdfUploadComponent {
  fileName = '';
  downloadedPdfName = '';
  base64: any;
  helloString = '';
  baseUrl = environment.apiUrl;
  stringedBase = '';
  invalidFileTypeCheck = false;

  databaseBool = false;
  convertionButtonBool = false;
  deleteButtonBool = false;

  fileDoesExist = false;
  fileDoesNotExist = false;
  loading = false;

  mess: Message = {
    id: 1,
    base64: '',
  };

  cont: Content = {
    id: 1,
    pdfName: '',
    content: '',
  };

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {}

  onFileSelected(event: any) {
    this.invalidFileTypeCheck = false;
    this.convertionButtonBool = false;

    let targetEvent = event.target;
    let file: File = targetEvent.files[0];
    let fileReader: FileReader = new FileReader();
    console.log(file.type);
    if (file.type === 'application/pdf') {
      fileReader.onload = (e) => {
        this.base64 = fileReader.result;
      };
      this.downloadedPdfName = file.name;
      fileReader.readAsDataURL(file);
      console.log(file);
      this.convertionButtonBool = true;
      this.fileDoesNotExist = false;
    } else {
      this.invalidFileTypeCheck = true;
    }
  }

  downloadFile() {
    const base64Download = this.base64.replace(
      'data:application/pdf;base64,',
      ''
    );

    const byteArray = new Uint8Array(
      atob(base64Download)
        .split('')
        .map((char) => char.charCodeAt(0))
    );
    console.log(this.mess.base64);
    console.log(this.base64);
    console.log(base64Download);

    const file = new Blob([byteArray], { type: 'application/pdf' });
    const fileUrl = URL.createObjectURL(file);
    let fileName = 'downloaded pdf';
    let link = document.createElement('a');
    link.download = fileName;
    link.target = '_blank';
    link.href = fileUrl;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  async prepareBase64(base64: Blob) {
    this.mess.base64 = base64;
    console.log(this.mess);

    this.http
      .put(this.baseUrl + `Database/upload-pdf`, this.stringedBase)
      .subscribe(
        (data) => {
          console.log(`Post request succesfulll`, data);
        },
        (error) => {
          console.log(`Error: `, error);
        }
      );
  }

  async postgresPdfToSpeech(base64: Blob) {
    const text = await new Response(base64).text();

    this.cont.pdfName = this.downloadedPdfName;
    this.cont.content = text;
    console.log(this.cont);

    this.http
      .put(this.baseUrl + `Database/upload-pdf-json-with-id/1`, this.cont)
      .subscribe(
        (data) => {
          console.log(`Post request succesfulll`, data);
        },
        (error) => {
          console.log(`Error: `, error);
        }
      );
    this.http.get(this.baseUrl + `Pdf/pdf-to-speech-sync`).subscribe(
      (data) => {
        console.log(`Post request succesfulll`, data);
      },
      (error) => {
        this.fileDoesNotExist = true;
        console.log(this.fileDoesNotExist);
        this.loading = false;
        console.log(`Error: `, error);
      }
    );
  }

  async assembledPostgresConvertion() {
    await this.postgresPdfToSpeech(this.base64);
    console.log('PostgresPdfToSpeech method is done');
    this.getBool();
  }

  async whileCheckConvertion() {
    this.loading = true;
    const sleep = (milliseconds: any) => {
      return new Promise((resolve) => setTimeout(resolve, milliseconds));
    };

    await this.postgresPdfToSpeech(this.base64);
    console.log('PostgresPdfToSpeech method is done');

    let i = 0;
    do {
      await sleep(1500);
      this.getBool();
      i++;
      console.log(i);
    } while (this.databaseBool !== true && i < 9 && !this.fileDoesNotExist);
    this.loading = false;
    if (!this.fileDoesNotExist) {
      this.fileDoesExist = true;
    }
  }

  async deleteFile() {
    this.http
      .delete(this.baseUrl + `Pdf/Delete/${this.cont.pdfName}`)
      .subscribe(
        (data) => {
          console.log(`Delete request succesfulll`, data);
        },
        (error) => {
          console.log(`Error: `, error);
        }
      );
  }

  printName() {
    console.log(this.cont.pdfName);
  }

  async getBool() {
    const sleep = (milliseconds: any) => {
      return new Promise((resolve) => setTimeout(resolve, milliseconds));
    };

    var streamName = this.cont.pdfName.replace(`.pdf`, ``);
    console.log(streamName);

    this.http
      .get<boolean>(this.baseUrl + `Pdf/does-file-exist/${streamName}`)
      .subscribe((data: boolean) => {
        this.databaseBool = data;
        this.setWhileBool(data);
      });
  }

  setBool(bool: boolean) {
    if (bool) {
      this.fileDoesExist = true;
    } else if (!bool) {
      this.fileDoesNotExist = true;
    }

    this.databaseBool = bool;
    console.log(`${this.databaseBool}`);
  }

  setWhileBool(bool: boolean) {
    this.databaseBool = bool;
    console.log(`${this.databaseBool}`);
  }

  databaseCheck() {
    do {
      this.getBool();
    } while (this.databaseBool !== true);
  }

  async completeDownload() {
    let fileUrl = `${this.baseUrl}Pdf/DownloadByStream/${this.cont.pdfName}`;
    console.log(fileUrl);

    var streamName = this.cont.pdfName.replace(`.pdf`, ``);

    let fileName = `${streamName}`;
    let link = document.createElement('a');
    link.download = fileName;
    link.target = '_blank';
    link.href = fileUrl;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    const sleep = (milliseconds: any) => {
      return new Promise((resolve) => setTimeout(resolve, milliseconds));
    };
    await sleep(2000);
    this.deleteFile();
    this.convertionButtonBool = false;
    this.databaseBool = false;
    this.fileDoesExist = false;
    this.fileDoesNotExist = false;
  }

  async completeDownloadOfPdf() {
    let fileUrl = `${this.baseUrl}Pdf/DownloadTestFile`;
    console.log(fileUrl);

    let fileName = `HelloWorld.pdf`;
    let link = document.createElement('a');
    link.download = fileName;
    link.target = '_blank';
    link.href = fileUrl;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    const sleep = (milliseconds: any) => {
      return new Promise((resolve) => setTimeout(resolve, milliseconds));
    };
    await sleep(2000);
  }
}
