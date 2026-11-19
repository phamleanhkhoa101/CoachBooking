export interface BrandLogoProps {
  size?: number;
  className?: string;
  label?: string;
}

/**
 * CoachBooking vector brand mark (Coach & Ticket Crest) in Midnight Navy & Warm Amber.
 */
export function BrandLogo({ size = 40, className, label }: BrandLogoProps) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 64 64"
      width={size}
      height={size}
      fill="none"
      className={className}
      style={{ display: 'block', flexShrink: 0 }}
      role={label ? 'img' : undefined}
      aria-label={label}
      aria-hidden={label ? undefined : true}
    >
      <rect width="64" height="64" rx="15" fill="#172638" />
      <rect
        x="0.75"
        y="0.75"
        width="62.5"
        height="62.5"
        rx="14.25"
        stroke="rgba(242,163,75,0.32)"
        strokeWidth="1.5"
      />

      {/* Highway ribbon curving into perforated ticket stub */}
      <path
        d="M15.8 33.8C10.2 39.2 12.6 48.2 28.5 48.2H42.6A1.6 1.6 0 0 0 45.8 48.2H50.8A3 3 0 0 0 50.8 54.2H45.8A1.6 1.6 0 0 0 42.6 54.2H27.8C8.4 54.2 5.4 40.6 14.4 32.4Z"
        fill="#30445B"
      />
      <path
        d="M14.8 35.8C11.2 41.2 14.4 51.2 28.5 51.2H40.6"
        stroke="#0B1521"
        strokeWidth="1.6"
        strokeLinecap="round"
      />
      <rect x="43.5" y="49.7" width="1.5" height="3" rx="0.75" fill="#0B1521" />

      {/* Express coach front silhouette */}
      <path
        d="M24.2 12.5C24.7 10.6 25.6 10 27.2 10H36.8C38.4 10 39.3 10.6 39.8 12.5H24.2Z"
        fill="#F5F2EE"
      />
      <path
        d="M19.5 19H17.1C16.2 19 15.6 19.6 15.6 20.5V25C15.6 25.8 16.2 26.4 17.1 26.4H18.1C18.7 26.4 19 25.9 19 25.1V21.1H19.5V19Z"
        fill="#F5F2EE"
      />
      <path
        d="M44.5 19H46.9C47.8 19 48.4 19.6 48.4 20.5V25C48.4 25.8 47.8 26.4 46.9 26.4H45.9C45.3 26.4 45 25.9 45 25.1V21.1H44.5V19Z"
        fill="#F5F2EE"
      />
      <path
        d="M23.5 13.5H40.5C43 13.5 44.5 15 44.5 17.5V38.4C44.5 40.2 43.3 41.5 41.5 41.5V44H37.5V41.5H26.5V44H22.5V41.5C20.7 41.5 19.5 40.2 19.5 38.4V17.5C19.5 15 21 13.5 23.5 13.5Z"
        fill="#F5F2EE"
      />

      {/* Split panoramic windshield */}
      <path
        d="M22.5 16.4C21.8 16.4 21.3 16.9 21.3 17.7V29.1C21.3 29.9 21.9 30.4 22.7 30.4H31.3V16.4H22.5Z"
        fill="#0B1521"
      />
      <path
        d="M32.7 16.4V30.4H41.3C42.1 30.4 42.7 29.9 42.7 29.1V17.7C42.7 16.9 42.2 16.4 41.5 16.4H32.7Z"
        fill="#0B1521"
      />
      <path
        d="M23 28.2L28.4 18.6M25.4 28.6L29.6 21.1"
        stroke="#F5F2EE"
        strokeWidth="1.2"
        strokeLinecap="round"
        strokeOpacity="0.8"
      />

      {/* Warm Amber headlights */}
      <path
        d="M21.4 33C21.4 32.3 22 32 22.8 32.2L26.3 33.1C27 33.3 27.3 33.9 27 34.6C26.7 35.3 26 35.6 25.1 35.6H22.4C21.8 35.6 21.4 35.1 21.4 34.4V33Z"
        fill="#F2A34B"
      />
      <path
        d="M21.4 36.7H26.5V38.7H22.4C21.8 38.7 21.4 38.2 21.4 37.6V36.7Z"
        fill="#F2A34B"
      />
      <path
        d="M42.6 33C42.6 32.3 42 32 41.2 32.2L37.7 33.1C37 33.3 36.7 33.9 37 34.6C37.3 35.3 38 35.6 38.9 35.6H41.6C42.2 35.6 42.6 35.1 42.6 34.4V33Z"
        fill="#F2A34B"
      />
      <path
        d="M42.6 36.7H37.5V38.7H41.6C42.2 38.7 42.6 38.2 42.6 37.6V36.7Z"
        fill="#F2A34B"
      />

      {/* Grille and bumper */}
      <rect x="28.6" y="34.2" width="6.8" height="1.1" rx="0.55" fill="#0B1521" />
      <rect x="29.4" y="36.1" width="5.2" height="1" rx="0.5" fill="#0B1521" />
      <path d="M28.4 38.4H35.6L36.4 40.3H27.6L28.4 38.4Z" fill="#0B1521" />
    </svg>
  );
}
