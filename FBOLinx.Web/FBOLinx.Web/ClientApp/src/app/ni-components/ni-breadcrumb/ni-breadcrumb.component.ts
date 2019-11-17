import { Component, OnInit, Input } from '@angular/core';
import { Item } from './item';
import { FbopricesService } from '../../services/fboprices.service';
import { SharedService } from '../../layouts/shared-service';

@Component({
  selector: 'ni-breadcrumb',
  templateUrl: './ni-breadcrumb.component.html',
  styleUrls: ['./ni-breadcrumb.component.scss']
})
export class NiBreadcrumbComponent implements OnInit {
  @Input() menu: Item[] = [];
  @Input() separator: string = '/';
  @Input() style: string = 'default';//custom1 | custom2
    public currentPrices: any[];
    public currentFboPriceJetARetail: any;
    public currentFboPriceJetACost: any;
    public currentFboPrice100LLRetail: any;
    public currentFboPrice100LLCost: any;

    constructor(private fboPricesService: FbopricesService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        this.fboPricesService.getFbopricesByFboIdCurrent(this.sharedService.currentUser.fboId)
            .subscribe((data: any) => {
                this.currentPrices = data;
                this.currentFboPrice100LLCost = this.getCurrentPriceByProduct('100LL Cost');
                this.currentFboPrice100LLRetail = this.getCurrentPriceByProduct('100LL Retail');
                this.currentFboPriceJetACost = this.getCurrentPriceByProduct('JetA Cost');
                this.currentFboPriceJetARetail = this.getCurrentPriceByProduct('JetA Retail');
            });}

    private getCurrentPriceByProduct(product) {
        var result = { fboId: this.sharedService.currentUser.fboId, groupId: this.sharedService.currentUser.groupId, oid: 0 };
        for (let fboPrice of this.currentPrices) {
            if (fboPrice.product == product)
                result = fboPrice;
        }
        console.log(result);
        return result;
    }

  getClasses() {
    return {
      'custom-1': this.style === 'custom1',
      'custom-2': this.style === 'custom2'
    };
  }
}
