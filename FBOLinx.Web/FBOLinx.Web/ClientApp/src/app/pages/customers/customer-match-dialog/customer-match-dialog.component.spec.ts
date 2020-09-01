import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { CustomerMatchDialogComponent } from "./customer-match-dialog.component";

describe("CustomerMatchDialogComponent", () => {
  let component: CustomerMatchDialogComponent;
  let fixture: ComponentFixture<CustomerMatchDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerMatchDialogComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerMatchDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
