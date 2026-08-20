import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RentalRequestsComponent } from './rental-requests/rental-requests.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'tongkangku_fe';
}
