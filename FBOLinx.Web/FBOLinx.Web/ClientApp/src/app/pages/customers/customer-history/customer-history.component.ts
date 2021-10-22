import { CustomerHistoryDetailsComponent } from './../customer-history-details/customer-history-details.component';
import { ActivatedRoute } from '@angular/router';
import { CustomerinfobygroupService } from './../../../services/customerinfobygroup.service';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-customer-history',
  templateUrl: './customer-history.component.html',
  styleUrls: ['./customer-history.component.scss']
})
export class CustomerHistoryComponent implements OnInit {

    @Input() customerHistory : any;
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
              private route : ActivatedRoute ,
              public dialog: MatDialog) { }

  ngOnInit() {
      if(! this.customerHistory)
      {

          this.customerInfoByGroupService.getCustomerByGroupLogger(this.route.snapshot.paramMap.get('id')).subscribe(
              data=> {
                this.customerHistory = data
                this.customerHistoryDataSource = new MatTableDataSource(this.customerHistory)
              }

          );
      }
      else
      {
        this.customerHistoryDataSource = new MatTableDataSource(this.customerHistory)
      }


  }

  public applyFilter(filterValue: string) {
    this.customerHistoryDataSource.filter = filterValue
        .trim()
        .toLowerCase();
}

  onPageChanged(e: any) {
    sessionStorage.setItem('pageIndex', e.pageIndex);
}

openDetailsDialog(customer : any)
{
    this.customerInfoByGroupService.getCustomerByGroupLoggerData(customer.oid , customer.logType).subscribe(
        data=>{
            const dialogRef = this.dialog.open(CustomerHistoryDetailsComponent, {
                width: '550px',
                height: '300px',
                data: {
                    details :  data ,
                    logType : customer.logType
                }
              });

        }
    )
}
}
