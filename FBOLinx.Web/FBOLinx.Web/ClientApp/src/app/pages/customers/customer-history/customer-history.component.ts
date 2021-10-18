import { ActivatedRoute } from '@angular/router';
import { CustomerinfobygroupService } from './../../../services/customerinfobygroup.service';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-customer-history',
  templateUrl: './customer-history.component.html',
  styleUrls: ['./customer-history.component.scss']
})
export class CustomerHistoryComponent implements OnInit {

    public customerHistoryDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'time',
        'action',
        'change',
        'username',
        'role',
        'location'
    ];
    public resultsLength = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes = false;
    public pageIndex = 0;


  constructor(private customerInfoByGroupService : CustomerinfobygroupService ,
              private route : ActivatedRoute) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    console.log(id);
    this.customerInfoByGroupService.getCustomerByGroupLogger(id).subscribe(
        ( data : any)=> {
            console.log(data)
            this.customerHistoryDataSource = new MatTableDataSource (data) ;
        }
    )

  }

  public applyFilter(filterValue: string) {
    this.customerHistoryDataSource.filter = filterValue
        .trim()
        .toLowerCase();
}

  onPageChanged(e: any) {
    sessionStorage.setItem('pageIndex', e.pageIndex);
}
}
