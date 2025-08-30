-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th3 31, 2025 lúc 09:51 PM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `shop`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `admin`
--

CREATE TABLE `admin` (
  `ID` int(255) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `PhoneNumber` varchar(10) NOT NULL,
  `Status` enum('active','locked') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `category`
--

CREATE TABLE `category` (
  `ID` int(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Status` enum('active','disabled') NOT NULL,
  `CreateTime` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `category`
--

INSERT INTO `category` (`ID`, `Name`, `Description`, `Status`, `CreateTime`) VALUES
(4, 'Accessories and Applications', 'For things', 'active', '2025-03-17 00:05:57'),
(5, 'Face', 'For face\r\n', 'active', '2025-03-31 18:49:06'),
(6, 'Nail', 'For nail', 'disabled', '2025-03-17 00:06:07'),
(7, 'Fragrance', 'For perfume', 'active', '2025-03-17 00:06:34'),
(86564, 'Lips', 'For lips', 'active', '2025-03-17 00:05:41'),
(9859034, 'Eyes', 'For eye', 'active', '2025-03-17 00:04:43');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `customer`
--

CREATE TABLE `customer` (
  `ID` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Address` varchar(100) NOT NULL,
  `PhoneNumber` varchar(10) NOT NULL,
  `Status` enum('active','locked') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `customer`
--

INSERT INTO `customer` (`ID`, `Name`, `Email`, `Password`, `Address`, `PhoneNumber`, `Status`) VALUES
(1, 'Anna', 'abd@gmail.com', '123', 'qbqbqbvq  qe', '2323', 'locked'),
(2, 'Ngoc', 'abc@gmail.com', 'H123@123', ' qrh', '2323', 'active'),
(3, 'Nhi Phan', 'nhiphan4303@gmail.com', 'Hanhnhi0403', 'Thủ Dầu Một', '0934056140', 'active'),
(4, 'Nhi Phan', 'ph.hnhi0403@gmail.com', 'Hanhnhi@0403', 'Thủ Dầu Một', '0934056140', 'active'),
(15, 'Mai Anh', 'nhiphan03@gmail.com', 'Hanhnhi0403', 'Bến Cát', '0907080506', 'active');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `order`
--

CREATE TABLE `order` (
  `ID` int(255) NOT NULL,
  `CustomerID` int(11) DEFAULT NULL,
  `TotalPrice` decimal(10,0) NOT NULL,
  `Date` datetime NOT NULL,
  `Status` enum('active','done','cancelled') NOT NULL DEFAULT 'active',
  `Note` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `order`
--

INSERT INTO `order` (`ID`, `CustomerID`, `TotalPrice`, `Date`, `Status`, `Note`) VALUES
(1, 2, 335, '2025-03-16 17:01:29', 'active', 'Ship COD');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `orderdetail`
--

CREATE TABLE `orderdetail` (
  `ID` int(255) NOT NULL,
  `OrderID` int(255) NOT NULL,
  `ProductID` int(255) NOT NULL,
  `Quantity` bigint(255) NOT NULL,
  `Price` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `product`
--

CREATE TABLE `product` (
  `ID` int(255) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Price` decimal(10,0) NOT NULL,
  `Image` varchar(255) NOT NULL,
  `InStock` bigint(20) NOT NULL,
  `Status` enum('available','out of stock') NOT NULL,
  `CategoryID` int(255) DEFAULT NULL,
  `CreateTime` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `product`
--

INSERT INTO `product` (`ID`, `Name`, `Description`, `Price`, `Image`, `InStock`, `Status`, `CategoryID`, `CreateTime`) VALUES
(3, 'Violet Strawberry Rococo Eyeshadow Palette', '01 Starmoon Strawberry - Jewel-Toned Dream\r\n\r\nA dreamy blend of icy blues and soft pinks, each shade captures the enchanting essence of a fairytale. Metallic pinks sparkle like stardust, while pastel hues swirl with soft, romantic magic.', 56, '/assets/images/products/e1a4bce0-df7a-4398-a07f-0e2beaf9f89d.webp', 250, 'available', 9859034, '2025-03-14 01:56:11'),
(9, 'Strawberry Cupid Pressed Powder', 'Say goodbye to dullness. Soft Lavender neutralizes yellow undertones and brightens your complexion for a healthy glow.', 35, '/assets/images/products/08b5907d-6808-4473-a108-6da068f121ed.webp', 250, 'available', 5, '2025-03-15 15:16:48'),
(11, 'Little Angel Embossed Highlighter', 'L01 Eros\'s Fable - Rosy Pearl\r\n\r\nA dreamy pink shimmer that swirls with light, casting a soft, ethereal glow.', 30, '/assets/images/products/36f1a7f5-063b-499a-90c4-439568dabf84.webp', 150, 'available', 5, '2025-03-15 15:05:39'),
(12, 'Strawberry Cupid All Day Glow Liquid Blush', '01 Frosty Strawberry—Baby Pink\r\n\r\nSoft and delicate, Frosty Strawberry is the perfect shade for a fresh, just-pinched pink glow.', 22, '/assets/images/products/5a8b5f20-048f-4af6-aec2-03800b157799.webp', 200, 'available', 5, '2025-03-16 15:15:35'),
(14, 'Butterfly Cloud Collar Mascara', 'Ink Black gives your lashes bold volume and lift that lasts all day. The curved brush glides on smooth, defining each lash with a waterproof finish—perfect for everyday or a bold, dramatic look.', 20, '/assets/images/products/fcbefc55-1034-4478-9edc-47dc2d2a4383.webp', 150, 'available', 9859034, '2025-03-17 00:12:38'),
(15, 'Butterfly Cloud Collar Liquid Eyeliner', 'Intensely pigmented and effortlessly precise, Ink Black delivers bold, long-lasting definition for any eye look.', 15, '/assets/images/products/c05a37ef-e530-4d91-a822-783b233e0571.webp', 280, 'available', 9859034, '2025-03-17 00:13:27'),
(16, 'Creamy Strawberry Trio Lip Set', 'Spoil someone special with the lovely custom Creamy Strawberry Trio Lip Set.', 53, '/assets/images/products/ed149db7-0a77-4a08-9647-3fa8558e602f.webp', 250, 'available', 86564, '2025-03-17 00:14:18'),
(17, 'Butterfly Cloud Collar Collection Glossy Lipstick', 'C01: Birchleaf Pair - Nude Apricot\r\nMeet your new favorite nude. A soft, sheer apricot, Birchleaf Pear adds just the right warmth to any look—it’s the perfect everyday shade', 20, '/assets/images/products/2e572e87-3ba5-4849-afda-c31ab71c77ad.webp', 250, 'available', 86564, '2025-03-17 00:15:12'),
(18, 'Juicy Strawberry Trio Lip Set', 'Spoil someone special with the lovely custom Juicy Strawberry Trio Lip Set.', 72, '/assets/images/products/afb2d85e-1c44-497c-aec0-78bce2357406.webp', 250, 'available', 86564, '2025-03-17 00:16:09'),
(19, 'Midsummer Fairytales Perfume', 'Inspired by garden courtyard architecture and birdcages, the perfume bottle features romantic elements like fairies, flowers, and butterflies. It offers a delicate and enchanting fragrance, reminiscent of a stroll through a lush garden.\r\n\r\nMidsummer Fanta', 45, '/assets/images/products/935d5e38-924a-4119-b6d7-cb121310cfa7.webp', 100, 'available', 7, '2025-03-17 00:17:04'),
(20, 'Strawberry Cupid Solid Perfume', 'Lychee Bouquet—lychee rose fruity floral\r\n\r\nA little sweet, a little floral. Lychee Bouquet blends juicy lychee, berries, and rose, with a soft musk finish.\r\n\r\nTop Notes: Lychee, Black Currant, Berries, Lemon\r\nHeart Notes: Rose, Jasmine\r\nBase Note: Musk', 62, '/assets/images/products/448e3c7b-29fc-458b-9a7a-eb87b0c725b3.webp', 100, 'available', 7, '2025-03-17 00:17:51'),
(21, 'Strawberry Cupid Perfume', 'Lychee Bouquet—lychee rose fruity floral\r\n\r\nA little sweet, a little floral. Lychee Bouquet blends juicy lychee, berries, and rose, with a soft musk finish.\r\n\r\nTop Notes: Lychee, Black Currant, Berries, Lemon\r\nHeart Notes: Rose, Jasmine\r\nBase Note: Musk', 80, '/assets/images/products/623f9309-d9a8-4121-a2ef-78d6ce16bc6c.webp', 50, 'available', 7, '2025-03-17 00:19:12'),
(22, 'Strawberry Cupid Hand Mirror', '🎀A mirror fit for a fairytale. The sweet strawberry cake design, complete with piped ‘cream’ and strawberries, is paired with a golden Cupid for the perfect blend of sweetness and romance.', 25, '/assets/images/products/9591146b-bd75-466c-86ef-ee8a336f35e7.webp', 150, 'available', 4, '2025-03-17 00:20:32'),
(23, 'Strawberry Cupid Scented Hand Cream', 'Lychee Bouquet—lychee rose fruity floral\r\n\r\nA little sweet, a little floral. Lychee Bouquet blends juicy lychee, berries, and rose, with a soft musk finish.\r\n\r\nTop Notes: Lychee, Black Currant, Berries, Lemon\r\n\r\nHeart Notes: Rose, Jasmine\r\n\r\nBase Notes: M', 15, '/assets/images/products/c23ff08d-b0f8-44f9-999e-ea8cc274bf7f.webp', 250, 'available', 4, '2025-03-17 00:21:30'),
(24, 'Flower Knows Chocolate Leather Tote Bag', '🍫 This Chocolate Leather Tote Bag oozes timeless elegance with a modern flair — perfect for fashionistas who want to look fab without sacrificing practicality. Its chocolate-hued hue brings a luxe feel for a versatile accessory that can complement any ens', 95, '/assets/images/products/de193287-e0b3-431a-8a06-da84af3605dd.webp', 50, 'available', 4, '2025-03-17 00:22:14'),
(25, 'Little Angel Mini Powder Puff', '🎀 Made specially for applying blush & highlighter. The fluffy surface delivers the ultimate compatible application of powder-based products.', 5, '/assets/images/products/1deb97dc-93e5-4772-a7a2-24067d9aaff3.webp', 350, 'available', 4, '2025-03-17 00:23:08'),
(26, 'Lips serum', '', 15, '/assets/images/products/43deb5d6-a269-4300-82dc-2fe6b08269c9.webp', 50, 'out of stock', 86564, '2025-03-17 00:23:56'),
(27, 'Radiant Eyes & Cheeks Duo A', 'Spoil someone special with the lovely custom Radiant Eyes & Cheeks Duo A', 92, '/assets/images/products/3cec03aa-677c-46b9-a37f-a601f8b1b8b2.webp', 50, 'available', 9859034, '2025-03-17 00:25:59');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`ID`);

--
-- Chỉ mục cho bảng `customer`
--
ALTER TABLE `customer`
  ADD PRIMARY KEY (`ID`);

--
-- Chỉ mục cho bảng `order`
--
ALTER TABLE `order`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_order_customer` (`CustomerID`),
  ADD KEY `idx_customer_id` (`CustomerID`);

--
-- Chỉ mục cho bảng `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_Product_Category` (`CategoryID`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `category`
--
ALTER TABLE `category`
  MODIFY `ID` int(255) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9859035;

--
-- AUTO_INCREMENT cho bảng `customer`
--
ALTER TABLE `customer`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT cho bảng `order`
--
ALTER TABLE `order`
  MODIFY `ID` int(255) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=323;

--
-- AUTO_INCREMENT cho bảng `product`
--
ALTER TABLE `product`
  MODIFY `ID` int(255) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `order`
--
ALTER TABLE `order`
  ADD CONSTRAINT `fk_order_customer` FOREIGN KEY (`CustomerID`) REFERENCES `customer` (`ID`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `FK_Product_Category` FOREIGN KEY (`CategoryID`) REFERENCES `category` (`ID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
