export const TaskStatus = {
  Todo: 'Todo',
  InProgress: 'InProgress',
  Review: 'Review',
  Done: 'Done',
} as const;
export type TaskStatus = (typeof TaskStatus)[keyof typeof TaskStatus];

export const TaskPriority = {
  Low: 'Low',
  Medium: 'Medium',
  High: 'High',
  Urgent: 'Urgent',
} as const;
export type TaskPriority = (typeof TaskPriority)[keyof typeof TaskPriority];

export const UserRole = {
  User: 'User',
  Manager: 'Manager',
  Admin: 'Admin',
} as const;
export type UserRole = (typeof UserRole)[keyof typeof UserRole];

export interface User {
  id: string;
  name: string;
  email: string;
  role: UserRole;
  department?: string;
  avatar?: string;
  createdAt: string;
}

export interface Task {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  assignedTo?: User;
  createdBy: User;
  category?: Category;
  tags: string[];
  estimatedHours?: number;
  actualHours?: number;
  customerFeedback?: string;
  sentimentScore?: string;
  aiSummary?: string;
  aiSummaryGeneratedAt?: string;
  createdAt: string;
  updatedAt: string;
  dueDate?: string;
  commentCount: number;
}

export interface Category {
  id: string;
  name: string;
  description?: string;
  color: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

export interface AISuggestion {
  suggestedTitle?: string;
  suggestedPriority?: TaskPriority;
  suggestedCategory?: string;
  confidence: 'High' | 'Medium' | 'Low';
  processingTimeMs: number;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  assignedToId?: string;
  categoryId?: string;
  tags: string[];
  estimatedHours?: number;
  dueDate?: string;
  customerFeedback?: string;
}
