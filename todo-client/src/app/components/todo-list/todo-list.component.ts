import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TodoItem } from '../../models/todo-item.model';

@Component({
    selector: 'app-todo-list',
    templateUrl: './todo-list.component.html',
    styleUrl: './todo-list.component.css',
    standalone: false
})
export class TodoListComponent {
  @Input() todos: TodoItem[] = [];
  @Output() toggleComplete = new EventEmitter<TodoItem>();
  @Output() deleteTodo = new EventEmitter<number>();

  onToggle(item: TodoItem): void {
    this.toggleComplete.emit(item);
  }

  onDelete(id: number): void {
    this.deleteTodo.emit(id);
  }

  get pendingCount(): number {
    return this.todos.filter(t => !t.isComplete).length;
  }
}

