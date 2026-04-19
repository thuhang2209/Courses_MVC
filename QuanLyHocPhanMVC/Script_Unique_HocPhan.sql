/*
Script them rang buoc UNIQUE cho MaHocPhan
- Kiem tra va hien thi du lieu trung ma hoc phan
- Neu khong con trung thi tao unique constraint
*/

USE QuanLyHocPhan;
GO

/* 1) Kiem tra cac ma hoc phan dang bi trung */
SELECT MaHocPhan, COUNT(*) AS SoLuong
FROM dbo.HocPhan
GROUP BY MaHocPhan
HAVING COUNT(*) > 1;
GO

/*
2) Neu co trung ma hoc phan thi chay block ben duoi de xoa ban ghi trung.
   Logic: giu lai ban ghi co IDHocPhan nho nhat cho moi MaHocPhan,
   xoa cac ban ghi con lai.
   LUU Y: Vi co khoa ngoai PhanCongHocPhan -> HocPhan,
   can xoa phan cong cua cac ban ghi trung truoc khi xoa HocPhan.
*/

;WITH DuplicateRows AS
(
    SELECT
        IDHocPhan,
        MaHocPhan,
        ROW_NUMBER() OVER (PARTITION BY MaHocPhan ORDER BY IDHocPhan) AS rn
    FROM dbo.HocPhan
)
DELETE pc
FROM dbo.PhanCongHocPhan pc
INNER JOIN DuplicateRows d ON pc.IDHocPhan = d.IDHocPhan
WHERE d.rn > 1;
GO

;WITH DuplicateRows AS
(
    SELECT
        IDHocPhan,
        MaHocPhan,
        ROW_NUMBER() OVER (PARTITION BY MaHocPhan ORDER BY IDHocPhan) AS rn
    FROM dbo.HocPhan
)
DELETE hp
FROM dbo.HocPhan hp
INNER JOIN DuplicateRows d ON hp.IDHocPhan = d.IDHocPhan
WHERE d.rn > 1;
GO

/* 3) Tao unique constraint neu chua ton tai */
IF NOT EXISTS
(
    SELECT 1
    FROM sys.key_constraints
    WHERE [name] = 'UQ_HocPhan_MaHocPhan'
      AND [type] = 'UQ'
)
BEGIN
    ALTER TABLE dbo.HocPhan
    ADD CONSTRAINT UQ_HocPhan_MaHocPhan UNIQUE (MaHocPhan);
END
GO

/* 4) Kiem tra lai */
SELECT MaHocPhan, COUNT(*) AS SoLuong
FROM dbo.HocPhan
GROUP BY MaHocPhan
HAVING COUNT(*) > 1;
GO
