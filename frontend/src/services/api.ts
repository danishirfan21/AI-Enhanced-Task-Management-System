import axios, { type AxiosInstance } from 'axios';
import type { AuthResponse, Task, AISuggestion, CreateTaskRequest } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

class APIClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Add request interceptor to include auth token
    this.client.interceptors.request.use((config) => {
      const token = localStorage.getItem('token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });

    // Add response interceptor for error handling
    this.client.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          // Token expired or invalid
          localStorage.removeItem('token');
          localStorage.removeItem('user');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // Auth endpoints
  async login(email: string, password: string): Promise<AuthResponse> {
    const response = await this.client.post<AuthResponse>('/auth/login', { email, password });
    return response.data;
  }

  async register(name: string, email: string, password: string, department?: string): Promise<AuthResponse> {
    const response = await this.client.post<AuthResponse>('/auth/register', {
      name,
      email,
      password,
      department,
    });
    return response.data;
  }

  // Task endpoints
  async getTasks(): Promise<Task[]> {
    const response = await this.client.get<Task[]>('/tasks');
    return response.data;
  }

  async getTask(id: string): Promise<Task> {
    const response = await this.client.get<Task>(`/tasks/${id}`);
    return response.data;
  }

  async createTask(task: CreateTaskRequest): Promise<Task> {
    const response = await this.client.post<Task>('/tasks', task);
    return response.data;
  }

  async updateTask(id: string, task: CreateTaskRequest): Promise<Task> {
    const response = await this.client.put<Task>(`/tasks/${id}`, task);
    return response.data;
  }

  async deleteTask(id: string): Promise<void> {
    await this.client.delete(`/tasks/${id}`);
  }

  // AI endpoints
  async suggestTaskFields(description: string): Promise<AISuggestion> {
    const response = await this.client.post<AISuggestion>('/ai/suggest-fields', {
      description,
    });
    return response.data;
  }

  async generateTaskSummary(taskId: string): Promise<Task> {
    const response = await this.client.post<Task>(`/tasks/${taskId}/generate-summary`);
    return response.data;
  }
}

export const api = new APIClient();
