import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-favorite-icon',
  templateUrl: './favorite-icon.component.html',
  styleUrls: ['./favorite-icon.component.scss']
})
export class FavoriteIconComponent implements OnInit {
    @Input() favoriteData: any;
    @Input() isSaving: boolean = false;
    @Input() hasPadding: boolean = true;
    @Output() favoriteClick = new EventEmitter<any>();

    isFavorite = false;

    constructor() { }

    ngOnInit() {
        this.isFavorite = this.favoriteData.isFavorite;
    }
    toogleFavorite(): void{
        if(this.isSaving) return;

        this.isFavorite = !this.isFavorite;
        this.favoriteData.isFavorite = this.isFavorite;

        this.favoriteClick.emit(this.favoriteData);
    }
}
