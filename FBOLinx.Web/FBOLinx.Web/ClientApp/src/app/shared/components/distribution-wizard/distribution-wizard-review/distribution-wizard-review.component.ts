import {
  OnInit,
  Component,
  Inject,
  EventEmitter,
  Output
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router, NavigationEnd } from '@angular/router';
import { SharedService } from '../../../../layouts/shared-service';

@Component({
  selector: 'app-distribution-wizard-review',
  templateUrl: './distribution-wizard-review.component.html',
  styleUrls: ['./distribution-wizard-review.component.scss'],
  providers: [SharedService],
})
export class DistributionWizardReviewComponent {
  @Output() idChanged1: EventEmitter<any> = new EventEmitter();

  public navigationSubscription: any;
  public previewEmail: string;

  constructor(
    public dialogRef: MatDialogRef<DistributionWizardReviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private router: Router
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        // trick the Router into believing it's last link wasn't previously loaded
        this.router.navigated = false;
        // if you need to scroll back to top, here is the right place
        window.scrollTo(0, 0);
      }
    });
  }

  public closeDialog() {
    this.dialogRef.close();
  }
}
