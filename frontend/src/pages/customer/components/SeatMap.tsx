interface SeatMapProps {
  soCho: number;
  tenLoaiXe: string;
  gheDaDat: number[];
  selected: number[];
  onToggle: (seat: number) => void;
  disabled?: boolean;
}

/** Xe "Giường nằm" chia 2 tầng (nửa đầu / nửa sau); các loại khác chỉ 1 tầng. */
function isGiuongNam(tenLoaiXe: string): boolean {
  const normalized = tenLoaiXe
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/đ/g, 'd')
    .replace(/Đ/g, 'D')
    .toLowerCase();
  return normalized.includes('giuong');
}

interface Deck {
  title: string;
  seats: number[];
}

function buildDecks(soCho: number, tenLoaiXe: string): Deck[] {
  const all = Array.from({ length: Math.max(0, soCho) }, (_, index) => index + 1);
  if (isGiuongNam(tenLoaiXe) && soCho > 1) {
    const half = Math.ceil(soCho / 2);
    return [
      { title: 'Tầng 1 · nửa đầu xe', seats: all.slice(0, half) },
      { title: 'Tầng 2 · nửa sau xe', seats: all.slice(half) },
    ];
  }
  return [{ title: '', seats: all }];
}

export function SeatMap({ soCho, tenLoaiXe, gheDaDat, selected, onToggle, disabled = false }: SeatMapProps) {
  const booked = new Set(gheDaDat);
  const chosen = new Set(selected);
  const decks = buildDecks(soCho, tenLoaiXe);

  return (
    <div className="seatmap">
      <div className="seatmap-head">
        <div>
          <strong>{tenLoaiXe || 'Sơ đồ ghế'}</strong>
          <span className="text-muted"> · {soCho} chỗ</span>
        </div>
        <div className="seat-legend">
          <span className="legend-item">
            <span className="legend-dot legend-free" /> Trống
          </span>
          <span className="legend-item">
            <span className="legend-dot legend-selected" /> Đang chọn
          </span>
          <span className="legend-item">
            <span className="legend-dot legend-booked" /> Đã đặt
          </span>
        </div>
      </div>

      <div className="seatmap-decks">
        {decks.map((deck) => (
          <section key={deck.title || 'deck-1'} className="seat-deck">
            {deck.title ? <h4 className="seat-deck-title">{deck.title}</h4> : null}
            <div className="seat-grid">
              {deck.seats.map((seat) => {
                const isBooked = booked.has(seat);
                const isSelected = chosen.has(seat);
                const className = isBooked
                  ? 'seat seat-booked'
                  : isSelected
                    ? 'seat seat-selected'
                    : 'seat seat-free';
                return (
                  <button
                    key={seat}
                    type="button"
                    className={className}
                    disabled={isBooked || disabled}
                    aria-pressed={isSelected}
                    title={isBooked ? `Ghế ${seat} đã được đặt` : `Ghế ${seat}`}
                    onClick={() => onToggle(seat)}
                  >
                    {seat}
                  </button>
                );
              })}
            </div>
          </section>
        ))}
      </div>
    </div>
  );
}
