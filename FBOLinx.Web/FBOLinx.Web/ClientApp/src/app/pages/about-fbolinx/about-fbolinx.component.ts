import { Component, OnInit } from '@angular/core';

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
    window.open('https://outlook.office365.com/owa/calendar/FBOLinxSales@fuelerlinx.com/bookings/', '_blank').focus();
  }
}
