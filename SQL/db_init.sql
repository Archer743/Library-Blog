CREATE DATABASE IF NOT EXISTS library;
# DROP DATABASE library; 
USE library;

CREATE TABLE IF NOT EXISTS users(
	id INT PRIMARY KEY AUTO_INCREMENT,
	username VARCHAR(50) NOT NULL,
	email_address VARCHAR(100) NOT NULL UNIQUE,
	country VARCHAR(100) DEFAULT 'Unknown',
	birthdate DATE NOT NULL,
	gender ENUM('male', 'female', 'other') NOT NULL,
	`status` ENUM('student', 'teacher', 'other') NOT NULL
);

CREATE TABLE IF NOT EXISTS admins(	
	user_id INT NOT NULL,
	CONSTRAINT FK_admins_users FOREIGN KEY(user_id) REFERENCES users(id),
    CONSTRAINT PK_admins PRIMARY KEY (user_id)
);

CREATE TABLE IF NOT EXISTS experts(
	user_id INT NOT NULL,
	CONSTRAINT FK_experts_users FOREIGN KEY(user_id) REFERENCES users(id),
    CONSTRAINT PK_experts PRIMARY KEY (user_id)
);

CREATE TABLE IF NOT EXISTS books (
	id INT PRIMARY KEY AUTO_INCREMENT,
	title VARCHAR(50) NOT NULL,
	genre VARCHAR(100) DEFAULT 'Other',
	author VARCHAR(150) DEFAULT 'Unknown',
	page_count INT NOT NULL,
	year_of_publishing YEAR NOT NULL,
	fav_count INT DEFAULT 0,
	preview TEXT NOT NULL,
	book_link VARCHAR(250) DEFAULT 'None'
    # , image BLOB
);

CREATE TABLE IF NOT EXISTS fav_books (
	user_id INT NOT NULL,
	book_id INT NOT NULL,
	CONSTRAINT FK_fav_books_users FOREIGN KEY(user_id) REFERENCES users(id),
	CONSTRAINT FK_fav_books_books FOREIGN KEY(book_id) REFERENCES books(id),
	CONSTRAINT PK_fav_books PRIMARY KEY (user_id, book_id)
);

CREATE TABLE IF NOT EXISTS comments (
	id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    book_id INT NOT NULL,
    content VARCHAR(400) NOT NULL,
    fav_count INT DEFAULT 0,
    date_posted DATETIME NOT NULL,
    CONSTRAINT FK_comments_users FOREIGN KEY(user_id) REFERENCES users(id),
    CONSTRAINT FK_comments_books FOREIGN KEY(book_id) REFERENCES books(id)
);

CREATE TABLE IF NOT EXISTS liked_comments (
	user_id INT NOT NULL,
	comment_id INT NOT NULL,
	CONSTRAINT FK_liked_comments_users FOREIGN KEY(user_id) REFERENCES users(id),
	CONSTRAINT FK_liked_comments_comments FOREIGN KEY(comment_id) REFERENCES comments(id),
	CONSTRAINT PK_liked_comments PRIMARY KEY (user_id, comment_id)
);
