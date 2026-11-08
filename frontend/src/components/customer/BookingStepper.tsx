import React from 'react';
import './BookingStepper.css';

interface BookingStepperProps {
  currentStep: 1 | 2 | 3 | 4;
}

const STEPS = [
  { step: 1, title: 'Chọn chuyến xe', desc: 'Lịch trình & Giờ xuất bến' },
  { step: 2, title: 'Chọn chỗ ngồi', desc: 'Sơ đồ ghế & Điểm đón' },
  { step: 3, title: 'Thanh toán', desc: 'Thông tin & VietQR 24/7' },
  { step: 4, title: 'Vé điện tử', desc: 'Boarding Pass & QR lên xe' },
];

export function BookingStepper({ currentStep }: BookingStepperProps) {
  return (
    <div className="booking-stepper-wrap">
      <div className="booking-stepper-container">
        {STEPS.map((item, idx) => {
          const isDone = item.step < currentStep;
          const isActive = item.step === currentStep;

          return (
            <React.Fragment key={item.step}>
              <div
                className={`stepper-node ${isDone ? 'done' : ''} ${
                  isActive ? 'active' : ''
                }`}
              >
                <div className="stepper-circle">
                  {isDone ? (
                    <svg
                      width="16"
                      height="16"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeWidth="3"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    >
                      <polyline points="20 6 9 17 4 12" />
                    </svg>
                  ) : (
                    <span>{item.step}</span>
                  )}
                </div>
                <div className="stepper-info">
                  <span className="stepper-title">{item.title}</span>
                  <span className="stepper-desc">{item.desc}</span>
                </div>
              </div>
              {idx < STEPS.length - 1 && (
                <div
                  className={`stepper-line ${
                    item.step < currentStep ? 'done' : ''
                  }`}
                />
              )}
            </React.Fragment>
          );
        })}
      </div>
    </div>
  );
}
