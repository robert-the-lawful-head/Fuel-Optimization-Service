import { ActivatedRoute } from '@angular/router';
import { SharedService } from 'src/app/layouts/shared-service';
import { CustomerHistoryDetailsComponent } from '../customer-history-details/customer-history-details.component';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';

@Component({
  selector: 'app-customer-history',
  templateUrl: './customer-history.component.html',
  styleUrls: ['./customer-history.component.scss']
})
export class CustomerHistoryComponent implements OnInit {

  @Input() customerHistory : any;
  customerHistoryDataSource : any;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  public pageIndex = 0;
  public resultsLength = 0;
  public displayedColumns: string[] = [
    'dateTime',
    'action',
    'changes',
    'username',
    'role',
    'tableName'
    ];

  constructor(public dialog: MatDialog,
     private sharedService : SharedService,
     private customerInfobyGroupService : CustomerinfobygroupService ,
     private route : ActivatedRoute) { }

  ngOnInit(): void {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
      this.customerHistoryDataSource = new MatTableDataSource(this.customerHistory);
    this.customerHistoryDataSource.sort = this.sort;
    this.customerHistoryDataSource.paginator = this.paginator;
    if (sessionStorage.getItem('pageIndex')) {
        this.paginator.pageIndex = sessionStorage.getItem(
            'pageIndex'
        ) as any;
        sessionStorage.removeItem('pageIndex');

    } else {
        this.paginator.pageIndex = 0;
    }

  }
  public applyFilter(filterValue: string) {
    this.customerHistoryDataSource.filter = filterValue.trim().toLowerCase();
}
openDetailsDialog(customer)
{
   this.dialog.open(CustomerHistoryDetailsComponent, {
        width: '500px',
        data: customer
      });

}

onPageChanged(e: any) {
    sessionStorage.setItem('pageIndex', e.pageIndex);
}
}
