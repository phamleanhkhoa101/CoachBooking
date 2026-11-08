import type { ReactNode } from 'react';

export interface SelectOption {
  value: string;
  label: string;
}

export type FormFieldType =
  | 'text'
  | 'email'
  | 'password'
  | 'number'
  | 'date'
  | 'time'
  | 'datetime-local'
  | 'select'
  | 'textarea'
  | 'checkbox';

interface FormFieldProps {
  label: string;
  value: string;
  onChange: (value: string) => void;
  name?: string;
  type?: FormFieldType;
  options?: SelectOption[];
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
  help?: string;
  error?: string;
  span?: 1 | 2;
  min?: number;
  max?: number;
  autoComplete?: string;
}

export function FormField({
  label,
  value,
  onChange,
  type = 'text',
  options = [],
  placeholder,
  required = false,
  disabled = false,
  help,
  error,
  span = 1,
  min,
  max,
  autoComplete,
}: FormFieldProps) {
  const fieldId = `field-${label
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/[^a-zA-Z0-9]+/g, '-')
    .toLowerCase()}`;

  const control = (): ReactNode => {
    if (type === 'select') {
      return (
        <select
          id={fieldId}
          value={value}
          disabled={disabled}
          required={required}
          onChange={(event) => onChange(event.target.value)}
        >
          <option value="">{placeholder ?? '— Chọn —'}</option>
          {options.map((opt) => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
      );
    }

    if (type === 'textarea') {
      return (
        <textarea
          id={fieldId}
          value={value}
          disabled={disabled}
          required={required}
          placeholder={placeholder}
          rows={3}
          onChange={(event) => onChange(event.target.value)}
        />
      );
    }

    if (type === 'checkbox') {
      const checked = value === 'true';
      return (
        <label className="switch" htmlFor={fieldId}>
          <input
            id={fieldId}
            type="checkbox"
            checked={checked}
            disabled={disabled}
            onChange={(event) => onChange(event.target.checked ? 'true' : 'false')}
          />
          <span className="switch-track" aria-hidden="true">
            <span className="switch-thumb" />
          </span>
          <span className="switch-label">{checked ? 'Có' : 'Không'}</span>
        </label>
      );
    }

    return (
      <input
        id={fieldId}
        type={type}
        value={value}
        disabled={disabled}
        required={required}
        placeholder={placeholder}
        min={min}
        max={max}
        autoComplete={autoComplete}
        onChange={(event) => onChange(event.target.value)}
      />
    );
  };

  return (
    <div className={`field ${span === 2 ? 'field-span-2' : ''}`}>
      <label htmlFor={fieldId}>
        {label}
        {required ? <span className="required-mark"> *</span> : null}
      </label>
      {control()}
      {help ? <span className="field-help">{help}</span> : null}
      {error ? <span className="field-error">{error}</span> : null}
    </div>
  );
}
