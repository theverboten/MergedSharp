import { Component, OnInit, Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrl: './uploader.component.css',
})
export class UploaderComponent implements OnInit {
  @Input() file: Blob | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropzoneOver = false;

  constructor() {}

  ngOnInit(): void {}
}
