import { Component } from '@angular/core';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';


@Component({
  selector: 'app-spinner-component',
  imports: [MatProgressSpinnerModule],
  templateUrl: './spinner-component.html',
  styleUrl: './spinner-component.css',
})
export class SpinnerComponent {}
