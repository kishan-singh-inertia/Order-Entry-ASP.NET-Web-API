SELECT
    COALESCE(p."ProductName", 'Total') AS "ProductName",
    SUM(CASE WHEN DATE_TRUNC('month', o."OrderDate") = DATE '2025-10-01' THEN oi."Quantity" * p."Price" ELSE 0 END) AS "Oct_2025",
    SUM(CASE WHEN DATE_TRUNC('month', o."OrderDate") = DATE '2025-09-01' THEN oi."Quantity" * p."Price" ELSE 0 END) AS "Sep_2025",
    SUM(CASE WHEN DATE_TRUNC('month', o."OrderDate") = DATE '2025-08-01' THEN oi."Quantity" * p."Price" ELSE 0 END) AS "Aug_2025",
    SUM(oi."Quantity" * p."Price") AS "Total"
FROM public."OrderItems" oi
JOIN public."Orders" o ON oi."OrderId" = o."OrderId"
JOIN public."Products" p ON oi."ProductId" = p."ProductId"
WHERE o."OrderDate" BETWEEN DATE '2025-08-01' AND DATE '2025-10-31'
GROUP BY ROLLUP(p."ProductName")
ORDER BY p."ProductName" NULLS LAST;