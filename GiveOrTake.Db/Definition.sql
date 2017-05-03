DROP SCHEMA IF EXISTS `GiveOrTake` ;

CREATE SCHEMA IF NOT EXISTS `GiveOrTake` DEFAULT CHARACTER SET utf8 ;
USE `GiveOrTake` ;

CREATE TABLE `User` (
  `UserId` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `UserName` VARCHAR(255) NOT NULL UNIQUE,
  `Password` TEXT NOT NULL,
  `Phone` VARCHAR(15) NOT NULL,
  PRIMARY KEY (`UserId`)) ;
  
CREATE TABLE `Item` (
  `ItemId` INT NOT NULL AUTO_INCREMENT,
  `ItemName` VARCHAR(255) NOT NULL,
  `UserId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`ItemId`),
  FOREIGN KEY (`UserId`) REFERENCES `User`(`UserId`));

CREATE TABLE `Transaction` (
  `TransactionId` INT NOT NULL PRIMARY KEY,
  `ShortDescription` TEXT,
  `OccurenceDate` DATETIME NOT NULL,
  `ExpectedReturnDate` DATETIME NOT NULL,
  `TransactionType` TINYINT(1) NOT NULL,
  `UserId` INT UNSIGNED NOT NULL,
  `ItemId` INT NOT NULL,
   FOREIGN KEY (`UserId`) REFERENCES `User`(`UserId`),
   FOREIGN KEY (`ItemId`)  REFERENCES `Item`(`ItemId`)) ;