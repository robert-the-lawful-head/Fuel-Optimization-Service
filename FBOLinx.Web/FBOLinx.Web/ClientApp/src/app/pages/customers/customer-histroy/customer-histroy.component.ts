import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-customer-histroy',
  templateUrl: './customer-histroy.component.html',
  styleUrls: ['./customer-histroy.component.scss']
})
export class CustomerHistroyComponent implements OnInit {

  @Input() customerHistroy : any;
  customerHistoryDataSource : any;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  public pageIndex = 0;
  public resultsLength = 0;
  public displayedColumns: string[] = [
    'dateTime',
    'type',
    'changes',
    'username',
    'tableName',



];
  constructor() { }

  ngOnInit(): void {

    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
    this.customerHistoryDataSource =   new MatTableDataSource(this.customerHistroy);
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


onPageChanged(e: any) {
    sessionStorage.setItem('pageIndex', e.pageIndex);
}
}
