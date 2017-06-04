SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `giveortake` ;
CREATE SCHEMA IF NOT EXISTS `giveortake` DEFAULT CHARACTER SET utf8 ;
SHOW WARNINGS;
USE `giveortake` ;

CREATE TABLE IF NOT EXISTS `giveortake`.`User` (
  `UserId` VARCHAR(255) NOT NULL,
  `FirstName` TEXT NOT NULL,
  `MiddleName` TEXT NULL,
  `LastName` TEXT NOT NULL,
  `Email` TEXT NOT NULL,
  PRIMARY KEY (`UserId`))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `giveortake`.`Item` (
  `ItemId` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NOT NULL,
  `UserId` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`ItemId`),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC),
  INDEX `fk_Item_User1_idx` (`UserId` ASC),
  CONSTRAINT `fk_Item_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `giveortake`.`User` (`UserId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `giveortake`.`Transaction` (
  `TransactionId` INT NOT NULL AUTO_INCREMENT,
  `Name` TEXT NOT NULL,
  `Description` TEXT NOT NULL,
  `OccurrenceDate` DATETIME NOT NULL,
  `ExpectedReturnDate` DATETIME NULL,
  `TransactionType` INT(1) NOT NULL,
  `ItemId` INT NOT NULL,
  `UserId` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`TransactionId`),
  INDEX `fk_Transaction_User1_idx` (`UserId` ASC),
  CONSTRAINT `fk_Transaction_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `giveortake`.`User` (`UserId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Transaction_Item1`
    FOREIGN KEY (`ItemId`)
    REFERENCES `giveortake`.`Item` (`ItemId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `giveortake`.`RootAccess` (
  `Password` TEXT NOT NULL,
  `UserId` VARCHAR(255) NOT NULL,
  INDEX `fk_RootAccess_User_idx` (`UserId` ASC),
  PRIMARY KEY (`UserId`),
  CONSTRAINT `fk_RootAccess_User`
    FOREIGN KEY (`UserId`)
    REFERENCES `giveortake`.`User` (`UserId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;