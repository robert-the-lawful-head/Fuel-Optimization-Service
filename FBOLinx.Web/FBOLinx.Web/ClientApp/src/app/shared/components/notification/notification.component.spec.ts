import { async, ComponentFixture, TestBed, ComponentFixtureAutoDetect } from '@angular/core/testing';

import { NotificationComponent } from './notification.component';
import { BrowserModule } from '@angular/platform-browser';


let component: NotificationComponent;
let fixture: ComponentFixture<NotificationComponent>;

describe('NotificationComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [NotificationComponent],
            imports: [BrowserModule],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(NotificationComponent);
        component = fixture.componentInstance;
    }));

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
