import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from '../environments/environment.development';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'client';
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  convertPdf() {
    this.http.get(this.baseUrl + `Pdf/pdf-to-speech`).subscribe(
      (data) => {
        console.log(`Convertion is successful `, data);
      },
      (error) => {
        console.log(`Error: `, error);
      }
    );
    console.log('Done succesfully in Angular');
  }
  /*
  testingAPI() {
    this.http.get(`http://localhost:5059/Image/test`);
    console.log('Text is printed from Angular');
  }*/
}
