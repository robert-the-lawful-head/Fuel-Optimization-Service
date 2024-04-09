import { Component, OnInit } from '@angular/core';
import { urls } from 'src/app/constants/externalUrlsConstants';


@Component({
  selector: 'app-about-fbolinx',
  templateUrl: './about-fbolinx.component.html',
  styleUrls: ['./about-fbolinx.component.scss']
})
export class AboutFbolinxComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }
  openRequestDemo() {
    window.open(urls.demoRequestUrl, '_blank').focus();
  }
}
