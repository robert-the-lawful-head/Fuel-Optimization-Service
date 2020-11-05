import { Component, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-group-analytics-generate-dialog',
  templateUrl: './group-analytics-generate-dialog.component.html',
  styleUrls: ['./group-analytics-generate-dialog.component.scss'],
})
export class GroupAnalyticsGenerateDialogComponent {
  @ViewChild('grid') public grid: GridComponent;

  // Public Members
  dataSources: any = {};
  filter: string = '';
  selectedCustomers: any[];
  loading: boolean;

  constructor(
    public dialogRef: MatDialogRef<GroupAnalyticsGenerateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.dataSources = data.customers
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onGenerate(): void {
    this.loading = true;

    const exportData = [
      {
        FBO: {
          FBO: ['F21', ['F12']],
        },
      },
      {
        FBO: {
          FBO: ['F21', ['F22']],
        }
      }
    ];

    // TODO
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Aircraft');

    /* save to file */
    XLSX.writeFile(wb, 'Aircraft.xlsx');
  }

  applyFilter(filter: string) {
    this.filter = filter;

    this.dataSources = this.data.customers.filter(customer =>
      customer.company.toLowerCase().includes(filter))
  }

  rowSelected() {
    this.selectedCustomers = this.grid.getSelectedRecords();
  }

  rowDeselected() {
    this.selectedCustomers = this.grid.getSelectedRecords();
  }
}
