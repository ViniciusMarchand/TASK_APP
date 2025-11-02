import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { Task } from '../../../interfaces';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { DialogData } from '../../home-page/home-page';
import { AuthService } from '../../../services/auth/auth-service';

@Component({
  selector: 'app-card',
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './card.html',
  styleUrl: './card.css',
})
export class Card {
  @Input() task!: Task;
  @Output() taskDeleted = new EventEmitter<string>();
  @Output() openEditModal = new EventEmitter<Task>();
  private authService = inject(AuthService);

  deleteTask() {
    this.taskDeleted.emit(this.task.id);
  }

  openModal() {
    this.openEditModal.emit(this.task);
  }

  getUserRole(): string {
    return this.authService.getUserRole();
  }

  canDelete(): boolean {
    if (this.getUserRole() == 'admin') {
      return true;
    }

    if (this.task.userId == this.authService.getUserId()) {
      return true;
    }

    return false;
  }

  canEdit(): boolean {
    const role = this.getUserRole();
    if (role == 'admin' || role == 'manager') {
      return true;
    }

    if (this.task.userId == this.authService.getUserId()) {
      return true;
    }

    return false;
  }
}
