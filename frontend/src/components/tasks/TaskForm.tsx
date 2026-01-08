import { useState, useEffect } from 'react';
import { api } from '../../services/api';
import type { CreateTaskRequest, TaskStatus, TaskPriority, AISuggestion } from '../../types';
import { Sparkles, Loader2 } from 'lucide-react';

interface TaskFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

export function TaskForm({ onSuccess, onCancel }: TaskFormProps) {
  const [formData, setFormData] = useState<CreateTaskRequest>({
    title: '',
    description: '',
    status: 'Todo' as TaskStatus,
    priority: 'Medium' as TaskPriority,
    tags: [],
    assignedToId: undefined,
    categoryId: undefined,
    estimatedHours: undefined,
    dueDate: undefined,
    customerFeedback: undefined,
  });

  const [aiSuggestions, setAiSuggestions] = useState<AISuggestion | null>(null);
  const [loadingSuggestions, setLoadingSuggestions] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  // Debounced AI suggestions
  useEffect(() => {
    const timer = setTimeout(async () => {
      if (formData.description.length >= 50) {
        await fetchAISuggestions();
      } else {
        setAiSuggestions(null);
      }
    }, 800); // Wait 800ms after user stops typing

    return () => clearTimeout(timer);
  }, [formData.description]);

  const fetchAISuggestions = async () => {
    setLoadingSuggestions(true);
    try {
      const suggestions = await api.suggestTaskFields(formData.description);
      setAiSuggestions(suggestions);
    } catch (err) {
      console.error('AI suggestions failed:', err);
    } finally {
      setLoadingSuggestions(false);
    }
  };

  const applySuggestion = (field: 'title' | 'priority' | 'category') => {
    if (!aiSuggestions) return;

    if (field === 'title' && aiSuggestions.suggestedTitle) {
      setFormData({ ...formData, title: aiSuggestions.suggestedTitle });
    } else if (field === 'priority' && aiSuggestions.suggestedPriority) {
      setFormData({ ...formData, priority: aiSuggestions.suggestedPriority });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    setError('');

    try {
      await api.createTask(formData);
      onSuccess();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to create task');
    } finally {
      setSubmitting(false);
    }
  };

  const confidenceColor = {
    High: 'bg-green-100 text-green-700 border-green-200',
    Medium: 'bg-yellow-100 text-yellow-700 border-yellow-200',
    Low: 'bg-orange-100 text-orange-700 border-orange-200',
  };

  return (
    <div className="bg-white rounded-lg shadow-lg p-6 max-w-2xl mx-auto">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">Create New Task</h2>

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Description - triggers AI */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Description *
            {loadingSuggestions && (
              <span className="ml-2 text-purple-600">
                <Loader2 className="inline w-4 h-4 animate-spin" />
                <span className="ml-1 text-xs">AI analyzing...</span>
              </span>
            )}
          </label>
          <textarea
            value={formData.description}
            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
            rows={4}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            placeholder="Describe the task in detail (min 50 characters for AI suggestions)..."
            required
          />
          {formData.description.length > 0 && formData.description.length < 50 && (
            <p className="text-sm text-gray-500 mt-1">
              {50 - formData.description.length} more characters for AI suggestions
            </p>
          )}
        </div>

        {/* AI Suggestions */}
        {aiSuggestions && (
          <div className="bg-gradient-to-r from-purple-50 to-cyan-50 border-2 border-purple-200 rounded-lg p-4 animate-fade-in">
            <div className="flex items-center gap-2 mb-3">
              <Sparkles className="w-5 h-5 text-purple-600" />
              <h3 className="font-semibold text-gray-900">AI Suggestions</h3>
              <span
                className={`text-xs px-2 py-1 rounded-full border ${
                  confidenceColor[aiSuggestions.confidence]
                }`}
              >
                {aiSuggestions.confidence} Confidence
              </span>
              <span className="text-xs text-gray-500 ml-auto">
                {aiSuggestions.processingTimeMs}ms
              </span>
            </div>

            <div className="space-y-2">
              {aiSuggestions.suggestedTitle && (
                <div
                  onClick={() => applySuggestion('title')}
                  className="ai-chip inline-flex"
                >
                  <span className="font-medium mr-2">Title:</span>
                  {aiSuggestions.suggestedTitle}
                </div>
              )}

              {aiSuggestions.suggestedPriority && (
                <div
                  onClick={() => applySuggestion('priority')}
                  className="ai-chip inline-flex ml-2"
                >
                  <span className="font-medium mr-2">Priority:</span>
                  {aiSuggestions.suggestedPriority}
                </div>
              )}

              {aiSuggestions.suggestedCategory && (
                <div className="ai-chip inline-flex ml-2">
                  <span className="font-medium mr-2">Category:</span>
                  {aiSuggestions.suggestedCategory}
                </div>
              )}
            </div>

            <p className="text-xs text-gray-600 mt-3">
              💡 Click on a suggestion to apply it
            </p>
          </div>
        )}

        {/* Title */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">Title *</label>
          <input
            type="text"
            value={formData.title}
            onChange={(e) => setFormData({ ...formData, title: e.target.value })}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            placeholder="Enter task title"
            required
          />
        </div>

        {/* Priority */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">Priority</label>
          <select
            value={formData.priority}
            onChange={(e) => setFormData({ ...formData, priority: e.target.value as TaskPriority })}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
          >
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
            <option value="Urgent">Urgent</option>
          </select>
        </div>

        {/* Status */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">Status</label>
          <select
            value={formData.status}
            onChange={(e) => setFormData({ ...formData, status: e.target.value as TaskStatus })}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
          >
            <option value="Todo">Todo</option>
            <option value="InProgress">In Progress</option>
            <option value="Review">Review</option>
            <option value="Done">Done</option>
          </select>
        </div>

        {error && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
            {error}
          </div>
        )}

        {/* Action buttons */}
        <div className="flex gap-3 pt-4">
          <button
            type="button"
            onClick={onCancel}
            className="flex-1 px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={submitting || loadingSuggestions}
            className="flex-1 bg-gradient-to-r from-purple-600 to-cyan-600 text-white py-2 rounded-lg font-semibold hover:from-purple-700 hover:to-cyan-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
          >
            {submitting ? 'Creating...' : 'Create Task'}
          </button>
        </div>
      </form>
    </div>
  );
}
