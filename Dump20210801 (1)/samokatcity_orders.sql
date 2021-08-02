-- MySQL dump 10.13  Distrib 8.0.25, for Win64 (x86_64)
--
-- Host: localhost    Database: samokatcity
-- ------------------------------------------------------
-- Server version	8.0.25

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DateStart` datetime NOT NULL,
  `DateEnd` datetime NOT NULL,
  `CountLock` int NOT NULL,
  `Discount` int NOT NULL,
  `AddBonuses` int NOT NULL,
  `StatusOrder` longtext,
  `Note` longtext,
  `Payment_Id` int DEFAULT NULL,
  `User_Id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Orders_Payments_Payment_Id` (`Payment_Id`),
  KEY `FK_Orders_Users_User_Id` (`User_Id`),
  CONSTRAINT `FK_Orders_Payments_Payment_Id` FOREIGN KEY (`Payment_Id`) REFERENCES `payments` (`Id`),
  CONSTRAINT `FK_Orders_Users_User_Id` FOREIGN KEY (`User_Id`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (33,'2021-07-29 10:45:00','2021-07-29 12:45:00',0,0,0,'Ожидает оплаты','',19,8),(34,'2021-07-29 17:15:00','2021-07-29 21:15:00',0,0,0,'Ожидает оплаты','Доплата 400 р.',20,10),(35,'2021-07-30 12:00:00','2021-07-30 14:00:00',0,0,0,'Ожидает оплаты','',21,9),(41,'2021-07-29 22:52:00','2021-07-30 00:52:00',0,0,0,'Ожидает оплаты','',24,11),(43,'2021-07-31 12:00:00','2021-08-02 00:00:00',7,700,0,'Ожидает оплаты','Доставка до Газпром арены 1900',26,9);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-08-01 11:42:35
