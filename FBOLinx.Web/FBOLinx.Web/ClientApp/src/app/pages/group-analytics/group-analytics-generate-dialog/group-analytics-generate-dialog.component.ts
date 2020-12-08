import { Component, Inject, ViewChild } from '@angular/core';
import * as XLSX from 'xlsx';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
  selector: 'app-group-analytics-generate-dialog',
  templateUrl: './group-analytics-generate-dialog.component.html',
  styleUrls: ['./group-analytics-generate-dialog.component.scss'],
  providers: [SharedService]
})
export class GroupAnalyticsGenerateDialogComponent {
  @ViewChild('grid') public grid: GridComponent;

  // Public Members
  dataSources: any = {};
  filter = '';
  selectedCustomers: any[];
  loading: boolean;
  exportedCustomersCount: number;

  constructor(
    public dialogRef: MatDialogRef<GroupAnalyticsGenerateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private customerInfoByGroupService: CustomerinfobygroupService,
    private sharedService: SharedService
  ) {
    this.dataSources = data.customers;
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  async onGenerate() {
    this.loading = true;
    this.exportedCustomersCount = 0;

    this.customerInfoByGroupService.getGroupAnalytics(
      this.sharedService.currentUser.groupId,
      this.selectedCustomers.map(customer => customer.customerId),
    ).subscribe(async (data: any[]) => {
      for (const customerFbos of data) {
        await this.exportReportForCustomer(customerFbos.company, customerFbos.groupCustomerFbos);
      }
      this.loading = false;
    });
  }

  applyFilter(filter: string) {
    this.filter = filter;

    this.dataSources = this.data.customers.filter(customer =>
      customer.company.toLowerCase().includes(filter));
  }

  rowSelected() {
    this.selectedCustomers = this.grid.getSelectedRecords();
  }

  rowDeselected() {
    this.selectedCustomers = this.grid.getSelectedRecords();
  }

  exportReportForCustomer(company: string, data: any) {
    return new Promise((resolve) => {
      const exportData: any[] = [];
      for (const fboPrice of data) {
        for (let i = 0; i < fboPrice.prices.length; i++) {
          exportData.push({
            FBO: i === 0 ? fboPrice.icao : '',
            'Volume Tier': fboPrice.prices[i].volumeTier,
            'Int/Comm': fboPrice.prices[i].intComm?.toFixed(4),
            'Int/Private': fboPrice.prices[i].intPrivate?.toFixed(4),
            'Dom/Comm': fboPrice.prices[i].domComm?.toFixed(4),
            'Dom/Private': fboPrice.prices[i].domPrivate?.toFixed(4),
          });
        }
        if (!fboPrice.prices.length) {
          exportData.push({
            FBO: fboPrice.icao,
            'Volume Tier': '',
            'Int/Comm': '',
            'Int/Private': '',
            'Dom/Comm': '',
            'Dom/Private': '',
          });
        }
      }

      // TODO
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Report');

      /* save to file */
      XLSX.writeFile(wb, company + '.csv');

      setTimeout(() => {
        resolve();
      }, 100);
    });
  }
}
