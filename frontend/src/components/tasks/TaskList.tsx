import { useEffect, useState } from 'react';
import { api } from '../../services/api';
import type { Task } from '../../types';
import { Plus, Loader2 } from 'lucide-react';

interface TaskListProps {
  onCreateTask: () => void;
}

export function TaskList({ onCreateTask }: TaskListProps) {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadTasks();
  }, []);

  const loadTasks = async () => {
    try {
      const data = await api.getTasks();
      setTasks(data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load tasks');
    } finally {
      setLoading(false);
    }
  };

  const getPriorityColor = (priority: string) => {
    const colors = {
      Low: 'bg-gray-100 text-gray-700',
      Medium: 'bg-blue-100 text-blue-700',
      High: 'bg-orange-100 text-orange-700',
      Urgent: 'bg-red-100 text-red-700',
    };
    return colors[priority as keyof typeof colors] || colors.Medium;
  };

  const getStatusColor = (status: string) => {
    const colors = {
      Todo: 'bg-gray-100 text-gray-700',
      InProgress: 'bg-yellow-100 text-yellow-700',
      Review: 'bg-purple-100 text-purple-700',
      Done: 'bg-green-100 text-green-700',
    };
    return colors[status as keyof typeof colors] || colors.Todo;
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <Loader2 className="w-8 h-8 animate-spin text-purple-600" />
        <span className="ml-2 text-gray-600">Loading tasks...</span>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
        {error}
      </div>
    );
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h2 className="text-2xl font-bold text-gray-900">Tasks</h2>
          <p className="text-gray-600">{tasks.length} total tasks</p>
        </div>
        <button
          onClick={onCreateTask}
          className="flex items-center gap-2 bg-gradient-to-r from-purple-600 to-cyan-600 text-white px-4 py-2 rounded-lg font-semibold hover:from-purple-700 hover:to-cyan-700 transition-all"
        >
          <Plus className="w-5 h-5" />
          New Task
        </button>
      </div>

      {tasks.length === 0 ? (
        <div className="text-center py-12 bg-gray-50 rounded-lg">
          <p className="text-gray-600 mb-4">No tasks yet</p>
          <button
            onClick={onCreateTask}
            className="text-purple-600 hover:text-purple-700 font-semibold"
          >
            Create your first task
          </button>
        </div>
      ) : (
        <div className="grid gap-4">
          {tasks.map((task) => (
            <div
              key={task.id}
              className="bg-white border border-gray-200 rounded-lg p-4 hover:shadow-md transition-shadow"
            >
              <div className="flex justify-between items-start">
                <div className="flex-1">
                  <h3 className="text-lg font-semibold text-gray-900 mb-2">{task.title}</h3>
                  <p className="text-gray-600 text-sm mb-3 line-clamp-2">{task.description}</p>

                  <div className="flex flex-wrap gap-2">
                    <span className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(task.status)}`}>
                      {task.status}
                    </span>
                    <span className={`px-2 py-1 rounded-full text-xs font-medium ${getPriorityColor(task.priority)}`}>
                      {task.priority}
                    </span>
                    {task.category && (
                      <span
                        className="px-2 py-1 rounded-full text-xs font-medium"
                        style={{
                          backgroundColor: `${task.category.color}20`,
                          color: task.category.color,
                        }}
                      >
                        {task.category.name}
                      </span>
                    )}
                    {task.aiSummary && (
                      <span className="ai-badge text-xs">
                        AI Summary
                      </span>
                    )}
                  </div>
                </div>

                <div className="text-right ml-4">
                  <p className="text-xs text-gray-500">
                    {task.createdBy.name}
                  </p>
                  <p className="text-xs text-gray-400 mt-1">
                    {new Date(task.createdAt).toLocaleDateString()}
                  </p>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
