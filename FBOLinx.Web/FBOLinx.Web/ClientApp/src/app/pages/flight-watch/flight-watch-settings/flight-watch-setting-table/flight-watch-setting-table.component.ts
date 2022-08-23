import { animate, state, style, transition, trigger } from '@angular/animations';
import { OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatSort, MatSortHeader, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SharedService } from 'src/app/layouts/shared-service';
import { FlightLegStatusEnum, Swim, swimTableColumns } from 'src/app/models/swim';
import {
    ColumnType,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { BooleanToTextPipe } from 'src/app/shared/pipes/boolean/booleanToText.pipe';
import { GetTimePipe } from 'src/app/shared/pipes/dateTime/getTime.pipe';
import { ToReadableDateTimePipe } from 'src/app/shared/pipes/dateTime/ToReadableDateTime.pipe';
import { ToReadableTimePipe } from 'src/app/shared/pipes/time/ToReadableTime.pipe';

@Component({
    selector: 'app-flight-watch-setting-table',
    templateUrl: './flight-watch-setting-table.component.html',
    styleUrls: ['./flight-watch-setting-table.component.scss'],
    animations: [
        trigger('detailExpand', [
            state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
            state('expanded', style({ height: '*' })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
            transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
          ])
    ]
})
export class FlightWatchSettingTableComponent implements OnInit {
    @Input() data: Swim[];
    @Input() isArrival: boolean;
    @Input() columns: ColumnType[];

    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() saveSettings = new EventEmitter();

    @ViewChild(MatSort) sort: MatSort;

    dataSource: MatTableDataSource<Swim>;

    columnsToDisplay : string[];

    columnsToDisplayWithExpand : any[];
    expandedElement: any | null;

    fbo: string;
    icao: string

    constructor(private getTime : GetTimePipe,
                private toReadableDateTime: ToReadableDateTimePipe,
                private toReadableTime: ToReadableTimePipe,
                private sharedService: SharedService,
                private booleanToText: BooleanToTextPipe) { }

    ngOnInit() {
        this.fbo = localStorage.getItem('fbo');
        this.icao = this.sharedService.currentUser.icao;
    }
    ngAfterViewInit() {
        this.dataSource = new MatTableDataSource(this.data);
        this.dataSource.sort = this.sort;

        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map((column) =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : {
                          hidden: column.hidden,
                          id: column.id,
                          name: column.name,
                      }
            );
            this.saveSettings.emit();
        });
    }
    ngOnChanges(changes: SimpleChanges) {
        if(changes.columns){
            this.columnsToDisplay = this.getVisibleColumns();
        }
        if(changes.data){
            this.dataSource.data = changes.data.currentValue;
        }
    }
    getVisibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.name) || []
        );
    }
    getMakeModelDisplayString(element: any){
        let str = (element.make)?element.make:"";
        str += (element.make && element.make)?"/":"";
        str += (element.model)?element.model:"";
        return str;
    }
    getOriginDestinationString(element: Swim){
        return this.isArrival
                ? element.origin
                : element.city
    }
    refreshSort() {
        let sortedColumn = this.columns.find(
            (column) => !column.hidden && column.sort
        );
        this.sort.sort({
            disableClear: false,
            id: null,
            start: sortedColumn?.sort || 'asc',
        });
        this.sort.sort({
            disableClear: false,
            id: sortedColumn?.name,
            start: sortedColumn?.sort || 'asc',
        });
        (
            this.sort.sortables.get(sortedColumn?.name) as MatSortHeader
        )?._setAnimationTransitionState({ toState: 'active' });
    }
    getSwimDataTypeString(){
        return (this.isArrival)? 'Arrivals': 'Departures';
    }
    getDateObject(dateString: string){
        return new Date(dateString);
    }
    getColumnData(row: Swim, column:string){
        console.log("🚀 ~ file: flight-watch-setting-table.component.ts ~ line 127 ~ FlightWatchSettingTableComponent ~ getColumnData ~ row", row.ete)
        if(column == "expandedDetail") return;
        if(column == swimTableColumns.originDestination) return this.getOriginDestinationString(row);
        if(column == swimTableColumns.ete) return this.toReadableTime.transform(row.ete);
        if(column == swimTableColumns.eta) return this.getTime.transform(this.getDateObject(row.etaLocal));
        if(column == swimTableColumns.isAircraftOnGround) return this.booleanToText.transform(row.isAircraftOnGround);
        if(column == swimTableColumns.status) {
            if(row.status == FlightLegStatusEnum.EnRoute)
                return "En Route";
            return  FlightLegStatusEnum[row.status];
        }
        let col = this.columns.find( c => c.name == column)
        return row[col.id];
    }
    getPastArrivalsValue(row: Swim){
            return this.isArrival
                ? row.arrivals
                : row.departures;

    }
    getColumnHeader(column: string){
            if( column != "Origin / Destination") return column;
            return this.isArrival
                ? 'Origin'
                : 'Destination';
    }
    sortData(sort: Sort) {
        console.log("🚀 ", this.expandedElement)
        this.dataSource.data.sort((a, b) => {
          const isAsc = sort.direction === 'asc';
          switch (sort.active) {
            case swimTableColumns.status:
              return this.compare(a.status, b.status, isAsc);
            case swimTableColumns.tailNumber:
              return this.compare(a.tailNumber, b.tailNumber, isAsc);
            case swimTableColumns.flightDepartment:
              return this.compare(a.flightDepartment, b.flightDepartment, isAsc);
            case swimTableColumns.icaoAircraftCode:
              return this.compare(a.icaoAircraftCode, b.icaoAircraftCode, isAsc);
            case swimTableColumns.ete:
              return this.compare(a.ete, b.ete, isAsc);
              case swimTableColumns.eta:
              return this.compare(a.etaZulu, b.etaZulu, isAsc);
              case swimTableColumns.originDestination:
                return (this.isArrival)? this.compare(a.arrivals, b.arrivals, isAsc): this.compare(a.departures, b.departures, isAsc);
              case swimTableColumns.isAircraftOnGround:
              return this.compare(a.isAircraftOnGround.toString(), b.isAircraftOnGround.toString(), isAsc);
              case swimTableColumns.itpMarginTemplate:
                return this.compare(a.itpMarginTemplate, b.itpMarginTemplate, isAsc);
            default:
              return 0;
          }
        });
    }
    compare(a: number | string , b: number | string, isAsc: boolean) {
        var result =  (a < b ? -1 : 1) * (isAsc ? 1 : -1);
        return result;

    }
}

export interface PeriodicElement {
    name: string;
    position: number;
    weight: number;
    symbol: string;
    description: string;
  }

  const ELEMENT_DATA: PeriodicElement[] = [
    {
      position: 1,
      name: 'Hydrogen',
      weight: 1.0079,
      symbol: 'H',
      description: `Hydrogen is a chemical element with symbol H and atomic number 1. With a standard
          atomic weight of 1.008, hydrogen is the lightest element on the periodic table.`,
    },
    {
      position: 2,
      name: 'Helium',
      weight: 4.0026,
      symbol: 'He',
      description: `Helium is a chemical element with symbol He and atomic number 2. It is a
          colorless, odorless, tasteless, non-toxic, inert, monatomic gas, the first in the noble gas
          group in the periodic table. Its boiling point is the lowest among all the elements.`,
    },
    {
      position: 3,
      name: 'Lithium',
      weight: 6.941,
      symbol: 'Li',
      description: `Lithium is a chemical element with symbol Li and atomic number 3. It is a soft,
          silvery-white alkali metal. Under standard conditions, it is the lightest metal and the
          lightest solid element.`,
    },
    {
      position: 4,
      name: 'Beryllium',
      weight: 9.0122,
      symbol: 'Be',
      description: `Beryllium is a chemical element with symbol Be and atomic number 4. It is a
          relatively rare element in the universe, usually occurring as a product of the spallation of
          larger atomic nuclei that have collided with cosmic rays.`,
    },
    {
      position: 5,
      name: 'Boron',
      weight: 10.811,
      symbol: 'B',
      description: `Boron is a chemical element with symbol B and atomic number 5. Produced entirely
          by cosmic ray spallation and supernovae and not by stellar nucleosynthesis, it is a
          low-abundance element in the Solar system and in the Earth's crust.`,
    },
    {
      position: 6,
      name: 'Carbon',
      weight: 12.0107,
      symbol: 'C',
      description: `Carbon is a chemical element with symbol C and atomic number 6. It is nonmetallic
          and tetravalent—making four electrons available to form covalent chemical bonds. It belongs
          to group 14 of the periodic table.`,
    },
    {
      position: 7,
      name: 'Nitrogen',
      weight: 14.0067,
      symbol: 'N',
      description: `Nitrogen is a chemical element with symbol N and atomic number 7. It was first
          discovered and isolated by Scottish physician Daniel Rutherford in 1772.`,
    },
    {
      position: 8,
      name: 'Oxygen',
      weight: 15.9994,
      symbol: 'O',
      description: `Oxygen is a chemical element with symbol O and atomic number 8. It is a member of
           the chalcogen group on the periodic table, a highly reactive nonmetal, and an oxidizing
           agent that readily forms oxides with most elements as well as with other compounds.`,
    },
    {
      position: 9,
      name: 'Fluorine',
      weight: 18.9984,
      symbol: 'F',
      description: `Fluorine is a chemical element with symbol F and atomic number 9. It is the
          lightest halogen and exists as a highly toxic pale yellow diatomic gas at standard
          conditions.`,
    },
    {
      position: 10,
      name: 'Neon',
      weight: 20.1797,
      symbol: 'Ne',
      description: `Neon is a chemical element with symbol Ne and atomic number 10. It is a noble gas.
          Neon is a colorless, odorless, inert monatomic gas under standard conditions, with about
          two-thirds the density of air.`,
    },
  ];
