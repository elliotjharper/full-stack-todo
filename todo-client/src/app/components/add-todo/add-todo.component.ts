import { Component, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-add-todo',
    templateUrl: './add-todo.component.html',
    styleUrl: './add-todo.component.css',
    standalone: false
})
export class AddTodoComponent {
  @Output() addTodo = new EventEmitter<string>();

  newTitle = '';

  onSubmit(): void {
    const title = this.newTitle.trim();
    if (!title) return;
    this.addTodo.emit(title);
    this.newTitle = '';
  }
}

