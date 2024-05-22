import { HttpClient } from '@angular/common/http';
import { Component, Injectable, Input, OnInit } from '@angular/core';
import {
  Observable,
  Subscription,
  catchError,
  delay,
  of,
  timer,
  fromEvent,
} from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Message } from './message';
import { environment } from '../environments/environment.development';
import { saveAs } from 'file-saver';
import { FileSaverOptions } from 'file-saver';
import { map, filter, switchMap } from 'rxjs/operators';
import { ajax } from 'rxjs/ajax';
import { buffer } from 'rxjs/operators';
import { Buffer } from 'buffer';
import { Content } from './content';
/*import FileSaver from 'file-saver';*/
import streamSaver from 'streamsaver';
import { concatMap } from 'rxjs/operators';
import { WritableStream } from 'web-streams-polyfill';
import { Params } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import * as PDFJSS from 'pdfjs-dist';
import { Url } from 'url';

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

  /*fileExists(url: string): Observable<boolean> {
    return this.http.get(url).pipe(
      map((response) => {
        return true;
      }),
      catchError((error) => {
        return of(false);
      })
    );
  }*/

  downloadFile() {
    /* this.base64 = this.base64.replace('data:application/pdf;base64,', '');*/
    const base64Download = this.base64.replace(
      'data:application/pdf;base64,',
      ''
    );

    const byteArray = new Uint8Array(
      atob(/*this.base64*/ base64Download)
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

    /*
    this.http.put(`http://localhost:3000/data/1`, this.mess).subscribe(
      (data) => {
        console.log(`Post request succesfulll`, data);
      },
      (error) => {
        console.log(`Error: `, error);
      }
    );*/

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
  /*
  async uploadStringToDatabase(base64: Blob) {
    const text = await new Response(base64).text();
    /*console.log(typeof text === 'string');*/
  /*  this.cont.content = text;
    console.log(this.cont);

    this.http
      .put(this.baseUrl + `Database/upload-pdf-json`, this.cont)
      .subscribe(
        (data) => {
          console.log(`Post request succesfulll`, data);
        },
        (error) => {
          console.log(`Error: `, error);
        }
      );
  }*/
  /*
  async pdfToSpeech(base64: Blob) {
    this.mess.base64 = base64;
    console.log(this.mess);

    this.http.put(`http://localhost:3000/data/1`, this.mess).subscribe(
      (data) => {
        console.log(`Post request succesfulll`, data);
      },
      (error) => {
        console.log(`Error: `, error);
      }
    );
    /* this.http.get(this.baseUrl + `Pdf/pdf-to-speech`);*/
  /* this.http.get(this.baseUrl + `Pdf/pdf-to-speech`).subscribe(
      (data) => {
        console.log(`Post request succesfulll`, data);
      },
      (error) => {
        console.log(`Error: `, error);
      }
    );
  }*/

  /******* */

  /*databaseCheck() {

    const myPromise: Promise<string> = 
     this.databaseRun().then()

     console.log(`${typeof value}`);
  }*/

  /* databaseCheck() {
    do {
      var value = this.databaseRun();
       = this.databaseBool;
     
      delay(1000);
    } while ( !== "true");
  }*/

  async postgresPdfToSpeech(base64: Blob) {
    const text = await new Response(base64).text();
    /*console.log(typeof text === 'string');*/
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
    /* delay(3000);*/
    /* this.http.get(this.baseUrl + `Pdf/pdf-to-speech`);*/
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
    /* delay(7000);*/
    /*
    this.http.delete(this.baseUrl + `Delete/${this.cont.pdfName}`).subscribe(
      (data) => {
        console.log(`Post request succesfulll`, data);
      },
      (error) => {
        console.log(`Error: `, error);
      }
    );*/
  }
  /*
  downloadConvertedFile() {
    let link = document.createElement('a');
    link.download = 'Speech';*/
  /* link.href = 'http://192.168.0.109:8080/ConvertedPdf.mp3';*/
  /* link.href = 'http://localhost:8080/ConvertedPdf.mp3';
    link.click();
    /*var URL = 'http://192.168.0.109:8080/ConvertedPdf.mp3';
    window.open(URL, '_blank');
  }

  async downloadPdfFile() {
    document.getElementById('download')?.click();
  } /*

  assembledConvertion() {
    this.prepareBase64(this.base64);
    delay(2000);
    console.log('Base64 is prepared');
    this.pdfToSpeech(this.base64);
    delay(2000);
    console.log('ConvertedPdf is prepared');
    this.downloadPdfFile();
    console.log('File is downloaded');
  }*/ /*

  async assembledWaitingConvertion() {
    await this.prepareBase64(this.base64);
    console.log('Base64 is prepared');

    await this.pdfToSpeech(this.base64);
    console.log('ConvertedPdf is prepared');

    await this.downloadPdfFile();
    console.log('File is downloaded');
  }*/

  /**** **** */

  async assembledPostgresConvertion() {
    /* await this.uploadStringToDatabase(this.base64); Done
    console.log('Base64 is prepared');*/

    await this.postgresPdfToSpeech(this.base64);
    console.log('PostgresPdfToSpeech method is done');
    /* await this.downloadPdfFile();
    console.log('File is downloaded');*/
    this.getBool();

    /* 'Soubor nebyl úspěšně konvertován. Zkontrolujte, jestli velikost vašeho PDF souboru skutečně nepřesahuje 3 stran';*/
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
      /*
      if (i >= 8) {
        this.fileDoesNotExist = true;
      }*/
    } while (this.databaseBool !== true && i < 9 && !this.fileDoesNotExist);
    this.loading = false;
    /* await sleep(800);*/
    if (!this.fileDoesNotExist) {
      this.fileDoesExist = true;
    }
  }

  /*
  async downloadPdf() {
    const res = await fetch(
      `http://localhost:5059/api/Pdf/DownloadByStream/${this.cont.pdfName}`,
      {
        method: `GET`,
      }
    );

    var streamName = this.cont.pdfName.replace(`.pdf`, ``);
    console.log(streamName);
    const fileStream = streamSaver.createWriteStream(`${streamName}.mp3`);
    const writer = fileStream.getWriter();

    const reader = res.body?.getReader();

    return writer.ready;
    
    const pump = () => reader?.read().then(({value, done}) => {
      if(done) writer.close();
      else{
        writer.write(value);
        return writer.ready.then(pump);
      }
    });

    await pump().then(()=> console.log(``);)*/

  /*
  async download(rs: ReadableStream) {
    var streamName = this.cont.pdfName.replace(`.pdf`, ``);
    var url = `http://localhost:5059/api/Pdf/DownloadByStream/${this.cont.pdfName}`;

    const res = await fetch(url, {
      method: `GET`,
    });

    const fileStream = streamSaver.createWriteStream(`${streamName}.mp3`);
    const writer = fileStream.getWriter();
  }*/

  async deleteFile() {
    /*
    this.http
      .get(this.baseUrl + `Pdf/DownloadByStream/${this.cont.pdfName}`)
      .subscribe(
        (data) => {
          console.log(`Get requet succesfull `, data);
        },
        (error) => {
          console.log(`Error: `, error);
        }
      );
    /** */

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

    /* await sleep(8000);*/
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
    /*  if (bool) {
      this.fileDoesExist = true;
    }*/
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
}
