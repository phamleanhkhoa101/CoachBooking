-- ========================================
-- ?? C?P NH?T GIÁ VÉ TH?C T? CHO CHUY?N XE
-- ========================================
-- Script này dùng ?? ??ng b? GiaVeThucTe t? b?ng GiaVe

-- ========================================
-- 1?? KI?M TRA TÌNH TR?NG HI?N T?I
-- ========================================
PRINT N'=== 1. KI?M TRA TÌNH TR?NG GIÁ VÉ ==='

-- S? chuy?n ch?a có giá vé
SELECT 
    COUNT(*) AS [T?ng s? chuy?n],
    SUM(CASE WHEN GiaVeThucTe IS NULL THEN 1 ELSE 0 END) AS [Ch?a có giá (NULL)],
    SUM(CASE WHEN GiaVeThucTe = 0 THEN 1 ELSE 0 END) AS [Giá vé = 0],
    SUM(CASE WHEN GiaVeThucTe > 0 THEN 1 ELSE 0 END) AS [?ã có giá vé]
FROM ChuyenXe

-- Chi ti?t chuy?n ch?a có giá
PRINT N''
PRINT N'=== Danh sách chuy?n ch?a có giá vé: ==='
SELECT 
    cx.MaChuyen,
    x.BienSoXe,
    td.DiemDi + N' ? ' + td.DiemDen AS Tuyen,
    cx.NgayGioKhoiHanh,
    cx.GiaVeThucTe,
    gv.GiaVe AS [Giá trong b?ng GiaVe]
FROM ChuyenXe cx
LEFT JOIN Xe x ON cx.MaXe = x.MaXe
LEFT JOIN TuyenDuong td ON cx.MaTuyen = td.MaTuyen
LEFT JOIN GiaVe gv ON gv.MaNX = x.MaNX 
                   AND gv.MaLX = x.MaLX 
                   AND gv.MaTuyen = cx.MaTuyen
WHERE cx.GiaVeThucTe IS NULL OR cx.GiaVeThucTe = 0
ORDER BY cx.NgayGioKhoiHanh

-- ========================================
-- 2?? T? ??NG C?P NH?T T? B?NG GIAVE
-- ========================================
PRINT N''
PRINT N'=== 2. T? ??NG C?P NH?T GIÁ VÉ ==='

-- Backup tr??c khi update (optional)
-- SELECT * INTO ChuyenXe_Backup_20250121 FROM ChuyenXe

-- C?p nh?t GiaVeThucTe t? b?ng GiaVe
UPDATE cx
SET cx.GiaVeThucTe = gv.GiaVe
FROM ChuyenXe cx
INNER JOIN Xe x ON cx.MaXe = x.MaXe
INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX 
                    AND gv.MaLX = x.MaLX 
                    AND gv.MaTuyen = cx.MaTuyen
WHERE cx.GiaVeThucTe IS NULL OR cx.GiaVeThucTe = 0

PRINT N'? ?ã c?p nh?t ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + N' chuy?n xe'

-- ========================================
-- 3?? KI?M TRA SAU KHI C?P NH?T
-- ========================================
PRINT N''
PRINT N'=== 3. KI?M TRA SAU C?P NH?T ==='

SELECT 
    COUNT(*) AS [T?ng s? chuy?n],
    SUM(CASE WHEN GiaVeThucTe IS NULL THEN 1 ELSE 0 END) AS [Ch?a có giá (NULL)],
    SUM(CASE WHEN GiaVeThucTe = 0 THEN 1 ELSE 0 END) AS [Giá vé = 0],
    SUM(CASE WHEN GiaVeThucTe > 0 THEN 1 ELSE 0 END) AS [?ã có giá vé]
FROM ChuyenXe

-- Chuy?n v?n ch?a có giá (không tìm th?y trong GiaVe)
PRINT N''
PRINT N'=== Chuy?n v?n ch?a có giá vé: ==='
SELECT 
    cx.MaChuyen,
    x.BienSoXe,
    x.MaNX,
    x.MaLX,
    cx.MaTuyen,
    td.DiemDi + N' ? ' + td.DiemDen AS Tuyen,
    cx.NgayGioKhoiHanh
FROM ChuyenXe cx
LEFT JOIN Xe x ON cx.MaXe = x.MaXe
LEFT JOIN TuyenDuong td ON cx.MaTuyen = td.MaTuyen
WHERE cx.GiaVeThucTe IS NULL OR cx.GiaVeThucTe = 0

IF @@ROWCOUNT > 0
BEGIN
    PRINT N'? CÒN CHUY?N CH?A CÓ GIÁ VÉ'
    PRINT N''
    PRINT N'? Nguyên nhân: Không tìm th?y c?u hình giá vé trong b?ng GiaVe'
    PRINT N'? Gi?i pháp:'
    PRINT N'   1. Ki?m tra b?ng GiaVe có record v?i (MaNX, MaLX, MaTuyen) t??ng ?ng không'
    PRINT N'   2. N?u không có, thêm vào GiaVe tr??c'
    PRINT N'   3. Ho?c c?p nh?t th? công cho t?ng chuy?n'
END
ELSE
BEGIN
    PRINT N'? T?T C? CHUY?N ?Ã CÓ GIÁ VÉ!'
END

-- ========================================
-- 4?? C?P NH?T TH? CÔNG (N?U C?N)
-- ========================================
PRINT N''
PRINT N'=== 4. C?P NH?T TH? CÔNG ==='

-- Template: C?p nh?t giá vé cho 1 chuy?n c? th?
/*
UPDATE ChuyenXe 
SET GiaVeThucTe = 350000 
WHERE MaChuyen = 1
*/

-- Template: C?p nh?t giá vé cho nhi?u chuy?n cùng tuy?n
/*
UPDATE ChuyenXe 
SET GiaVeThucTe = 350000 
WHERE MaTuyen = 1 
  AND (GiaVeThucTe IS NULL OR GiaVeThucTe = 0)
*/

-- ========================================
-- 5?? T?O TRIGGER T? ??NG (KHUY?N NGH?)
-- ========================================
PRINT N''
PRINT N'=== 5. T?O TRIGGER T? ??NG ==='

-- Ki?m tra trigger ?ã t?n t?i ch?a
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'trg_ChuyenXe_AutoSetGiaVe')
BEGIN
    PRINT N'?? Trigger ?ã t?n t?i, ?ang xóa ?? t?o l?i...'
    DROP TRIGGER trg_ChuyenXe_AutoSetGiaVe
END

GO

-- T?o trigger t? ??ng c?p nh?t GiaVeThucTe
CREATE TRIGGER trg_ChuyenXe_AutoSetGiaVe
ON ChuyenXe
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- T? ??ng c?p nh?t GiaVeThucTe n?u NULL ho?c 0
    UPDATE cx
    SET cx.GiaVeThucTe = gv.GiaVe
    FROM ChuyenXe cx
    INNER JOIN INSERTED i ON cx.MaChuyen = i.MaChuyen
    INNER JOIN Xe x ON cx.MaXe = x.MaXe
    INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX 
                        AND gv.MaLX = x.MaLX 
                        AND gv.MaTuyen = cx.MaTuyen
    WHERE (i.GiaVeThucTe IS NULL OR i.GiaVeThucTe = 0)
      AND gv.GiaVe IS NOT NULL
      AND gv.GiaVe > 0;
      
    -- Log n?u không tìm th?y giá vé
    IF EXISTS (
        SELECT 1 
        FROM INSERTED i
        INNER JOIN Xe x ON i.MaXe = x.MaXe
        LEFT JOIN GiaVe gv ON gv.MaNX = x.MaNX 
                           AND gv.MaLX = x.MaLX 
                           AND gv.MaTuyen = i.MaTuyen
        WHERE (i.GiaVeThucTe IS NULL OR i.GiaVeThucTe = 0)
          AND gv.GiaVe IS NULL
    )
    BEGIN
        RAISERROR (N'C?nh báo: Không tìm th?y giá vé trong b?ng GiaVe. Vui lòng c?p nh?t GiaVeThucTe th? công.', 16, 1);
    END
END

GO

PRINT N'? ?ã t?o trigger trg_ChuyenXe_AutoSetGiaVe'

-- ========================================
-- 6?? TEST TRIGGER
-- ========================================
PRINT N''
PRINT N'=== 6. TEST TRIGGER ==='

-- Test insert chuy?n m?i (không ?i?n GiaVeThucTe)
/*
-- L?y thông tin xe và tuy?n có s?n
DECLARE @MaXe INT = (SELECT TOP 1 MaXe FROM Xe)
DECLARE @MaTuyen INT = (SELECT TOP 1 MaTuyen FROM TuyenDuong)
DECLARE @MaTX INT = (SELECT TOP 1 MaTX FROM TaiXe)

-- Insert chuy?n m?i
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh, NgayGioDenDuKien, GiaVeThucTe)
VALUES (@MaXe, @MaTuyen, @MaTX, '2025-04-22 08:00', '2025-04-22 16:00', NULL)

-- Ki?m tra GiaVeThucTe ?ã ???c t? ??ng fill ch?a
SELECT TOP 1 MaChuyen, MaXe, MaTuyen, GiaVeThucTe 
FROM ChuyenXe 
ORDER BY MaChuyen DESC

-- Xóa test data
DELETE FROM ChuyenXe WHERE MaChuyen = (SELECT MAX(MaChuyen) FROM ChuyenXe)
*/

-- ========================================
-- 7?? TH?NG KÊ CU?I CÙNG
-- ========================================
PRINT N''
PRINT N'=== 7. TH?NG KÊ CU?I CÙNG ==='

SELECT 
    N'T?ng s? chuy?n' AS [Lo?i],
    COUNT(*) AS [S? l??ng],
    CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM ChuyenXe) AS DECIMAL(5,2)) AS [T? l? %]
FROM ChuyenXe

UNION ALL

SELECT 
    N'?ã có giá vé',
    COUNT(*),
    CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM ChuyenXe) AS DECIMAL(5,2))
FROM ChuyenXe
WHERE GiaVeThucTe > 0

UNION ALL

SELECT 
    N'Ch?a có giá vé',
    COUNT(*),
    CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM ChuyenXe) AS DECIMAL(5,2))
FROM ChuyenXe
WHERE GiaVeThucTe IS NULL OR GiaVeThucTe = 0

-- ========================================
-- ?? K?T LU?N
-- ========================================
PRINT N''
PRINT N'=== K?T LU?N ==='
PRINT N'? Script ch?y xong!'
PRINT N'? Trigger ?ã ???c t?o, giá vé s? t? ??ng c?p nh?t khi thêm chuy?n m?i'
PRINT N'? N?u v?n còn chuy?n ch?a có giá, hãy:'
PRINT N'   1. Thêm c?u hình vào b?ng GiaVe'
PRINT N'   2. Ho?c c?p nh?t th? công GiaVeThucTe cho t?ng chuy?n'
