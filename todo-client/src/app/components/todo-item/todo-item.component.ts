import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TodoItem } from '../../models/todo-item.model';

@Component({
    selector: 'app-todo-item',
    templateUrl: './todo-item.component.html',
    styleUrl: './todo-item.component.css',
    standalone: false
})
export class TodoItemComponent {
  @Input() todo!: TodoItem;
  @Output() toggleComplete = new EventEmitter<TodoItem>();
  @Output() deleteTodo = new EventEmitter<number>();

  onToggle(): void {
    this.toggleComplete.emit(this.todo);
  }

  onDelete(): void {
    this.deleteTodo.emit(this.todo.id);
  }
}

