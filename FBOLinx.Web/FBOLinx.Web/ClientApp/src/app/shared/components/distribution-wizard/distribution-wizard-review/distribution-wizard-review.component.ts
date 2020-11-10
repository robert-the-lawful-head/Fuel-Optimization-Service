import {
  OnInit,
  Component,
  Inject,
  ViewChild,
  EventEmitter,
  Output,
  ElementRef,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PricingtemplatesService } from '../../../../services/pricingtemplates.service';
import { RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';
import { Router, NavigationEnd } from '@angular/router';
import { FbopricesService } from '../../../../services/fboprices.service';
import { SharedService } from '../../../../layouts/shared-service';

export interface TailLookupResponse {
  template?: string;
  company?: string;
  makeModel?: string;
  pricingList: Array<any>;
  rampFee: any;
}

@Component({
  selector: 'app-distribution-wizard-review',
  templateUrl: './distribution-wizard-review.component.html',
  styleUrls: ['./distribution-wizard-review.component.scss'],
  providers: [SharedService],
})
export class DistributionWizardReviewComponent implements OnInit {
  public edit = false;
  public customerMargins: any[];
  public currentPrice: any[];
  public navigationSubscription: any;

  @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;
  @Output() idChanged1: EventEmitter<any> = new EventEmitter();
  @ViewChild('custMargin') divView: ElementRef;

  constructor(
    public dialogRef: MatDialogRef<DistributionWizardReviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private pricingTemplatesService: PricingtemplatesService,
    private router: Router,
    private fbopricesService: FbopricesService,
    private sharedService: SharedService
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };

    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        // trick the Router into believing it's last link wasn't previously loaded
        this.router.navigated = false;
        // if you need to scroll back to top, here is the right place
        window.scrollTo(0, 0);
      }
    });
  }

  ngOnInit() {
    try {
      this.loadCustomerForPreview();
    } catch (e) {
      alert(e.message);
    }
  }

  public changeStatus() {
    this.edit = true;
  }

  public update() {
    this.pricingTemplatesService
      .update(this.data)
      .subscribe((data: any) => {
        this.edit = false;
      });
  }

  public disableToolbarEmail() {
    this.rteEmail.toolbarSettings.enable = false;
  }

  public enableToolbarEmail() {
    this.rteEmail.toolbarSettings.enable = true;
  }

  public closeDialog() {
    this.dialogRef.close();
  }

  public editPricingTemplate(pricingTemplate) {
    this.dialogRef.close();
    this.idChanged1.emit(null);
    // const clonedRecord = Object.assign({}, pricingTemplate);
    // this.router.navigate(['/default-layout/pricing-templates/' + pricingTemplate.oid]);
    this.router
      .navigateByUrl(
        '/default-layout/pricing-templates/' + pricingTemplate.oid
      )
      .then((e) => {
        if (e) {
          this.router.navigateByUrl(this.router.url);
        } else {
          this.router.navigateByUrl('./' + pricingTemplate.oid);
        }
      });
  }
  private loadCustomerForPreview() {
    var templateId = this.data.oid;

    const tailLookupData = {
      icao: this.sharedService.currentUser.icao,
      fboId: this.sharedService.currentUser.fboId,
      groupId: this.sharedService.currentUser.groupId,
      pricingTemplateID: templateId
    };
    this.fbopricesService.getFuelPricesForCompany(tailLookupData).subscribe((data: TailLookupResponse) => {
      if (data) {
        this.customerMargins = data.pricingList;
      }
    },
      (error: any) => {
        console.log(error);
      });

    //this.customerInfoByGroupService
    //  .getCustomersByGroupAndFBOAndPricing(
    //    groupId,
    //    fboId,
    //    templateId
    //  )
    //  .subscribe((data: any) => {
    //    if (data.length > 0) {
    //      for (const customer of data) {
    //        this.customerAircraftsService
    //          .getCustomerAircraftsByGroupAndCustomerId(
    //            groupId,
    //            fboId,
    //            customer.customerId
    //          )
    //          .subscribe((data: any) => {
    //            var customerAircrafts = data;

    //            if (customerAircrafts.length > 0) {
    //              for (const aircraft of customerAircrafts) {
    //                if (aircraft.pricingTemplateId == 0) {
    //                  tailNumber = aircraft.tailNumber;
    //                  customerId = customer.oid;

    //                  if (tailNumber != "") {
    //                    const tailLookupData = {
    //                      icao: this.sharedService.currentUser.icao,
    //                      tailNumber: tailNumber,
    //                      fboId: this.sharedService.currentUser.fboId,
    //                      groupId: this.sharedService.currentUser.groupId,
    //                      customerInfoByGroupId: customerId
    //                    };
    //                    this.fbopricesService.getFuelPricesForCompany(tailLookupData).subscribe((data: TailLookupResponse) => {
    //                      if (data) {
    //                        this.customerMargins = data.pricingList;
    //                      }
    //                    },
    //                      (error: any) => {
    //                        console.log(error);
    //                      });
    //                    break;
    //                  }
    //                }
    //              }
    //            }
    //            else {
    //              const tailLookupData = {
    //                icao: this.sharedService.currentUser.icao,
    //                fboId: this.sharedService.currentUser.fboId,
    //                groupId: this.sharedService.currentUser.groupId,
    //                pricingTemplateID: templateId
    //              };
    //              this.fbopricesService.getFuelPricesForCompany(tailLookupData).subscribe((data: TailLookupResponse) => {
    //                if (data) {
    //                  this.customerMargins = data.pricingList;
    //                }
    //              },
    //                (error: any) => {
    //                  console.log(error);
    //                });
    //            }
    //          },
    //            (error: any) => {
    //              console.log(error);
    //            });

    //        if (this.customerMargins.length > 0)
    //          break;
    //      }
    //    }
    //  },
    //    (error: any) => {
    //      console.log(error);
    //    });
  }
}
