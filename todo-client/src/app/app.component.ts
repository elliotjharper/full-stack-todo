import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TodoService } from './services/todo.service';
import { TodoItem } from './models/todo-item.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  todos: TodoItem[] = [];
  isLoading = false;
  errorMessage: string | null = null;

  private destroy$ = new Subject<void>();

  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    this.loadTodos();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadTodos(): void {
    this.isLoading = true;
    this.errorMessage = null;
    this.todoService.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: todos => {
          this.todos = todos;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to load todos. Is the API running?';
          this.isLoading = false;
        }
      });
  }

  onAddTodo(title: string): void {
    this.todoService.create({ title, isComplete: false })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: newItem => {
          this.todos = [...this.todos, newItem];
        },
        error: () => {
          this.errorMessage = 'Failed to add todo.';
        }
      });
  }

  onToggleComplete(item: TodoItem): void {
    this.todoService.toggleComplete(item)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: updated => {
          this.todos = this.todos.map(t => t.id === updated.id ? updated : t);
        },
        error: () => {
          this.errorMessage = 'Failed to update todo.';
        }
      });
  }

  onDeleteTodo(id: number): void {
    this.todoService.delete(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.todos = this.todos.filter(t => t.id !== id);
        },
        error: () => {
          this.errorMessage = 'Failed to delete todo.';
        }
      });
  }
}

