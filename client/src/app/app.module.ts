import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { PdfUploadComponent } from '../pdf-upload/pdf-upload.component';
import { DesignComponent } from './design/design.component';
import { UploaderComponent } from './uploader/uploader.component';
import { FileUploadModule } from 'ng2-file-upload';
import { FormsModule } from '@angular/forms';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { PagenotfoundComponent } from './pagenotfound/pagenotfound.component';

/*import { File.componentComponent } from './file.component/file.component.component';
import { Pdf.uploadComponent } from '.c:/Users/hp/Desktop/SharpTest/client/src/pdf.upload/pdf.upload.component';
import { PdfUploadComponent } from '.c:/Users/hp/Desktop/SharpTest/client/src/pdf-upload/pdf-upload.component';*/

@NgModule({
  declarations: [
    AppComponent,
    /* File.componentComponent,
      Pdf.uploadComponent,*/
    PdfUploadComponent,
    DesignComponent,
    UploaderComponent,
    PagenotfoundComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    PdfViewerModule,
    FileUploadModule,
    FormsModule,
  ],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  bootstrap: [AppComponent],
  exports: [FileUploadModule],
})
export class AppModule {}
