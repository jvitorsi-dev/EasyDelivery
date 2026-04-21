import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  getRouteState(outlet: RouterOutlet) {
    return outlet.activatedRouteData?.['animation'] ?? '';
  }
  protected readonly title = signal('easydelivery-front');
}
