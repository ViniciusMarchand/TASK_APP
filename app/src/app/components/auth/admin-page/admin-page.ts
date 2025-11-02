import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { AuthService } from '../../../services/auth/auth-service';
import { User } from '../../../interfaces';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-admin-page',
  imports: [MatButtonModule, MatTableModule, MatSelectModule],
  templateUrl: './admin-page.html',
  styleUrl: './admin-page.css',
})
export class AdminPage {
  displayedColumns: string[] = ["id", "name", "username", "email", "role"];
  private cdRef = inject(ChangeDetectorRef);

  private authService = inject(AuthService);

  users: User[] = [];

  ngOnInit(): void {
    this.authService.findAllUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.cdRef.markForCheck()
      },
      error: (error) => {
        console.error(error.message)
        this.users = [];
      },
    });
  }

  assignUserRole(role:string, userId:string) {
    this.authService.assignUserRole(role, userId).subscribe(({
      error: (error) => console.log(error.message)
    }))
  }
}
