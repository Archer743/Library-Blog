CREATE DATABASE IF NOT EXISTS library;
USE library;

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
);

CREATE TABLE IF NOT EXISTS fav_books (
	user_id INT NOT NULL,
	book_id INT NOT NULL,
	CONSTRAINT FK_fav_books_users FOREIGN KEY(user_id) REFERENCES users(user_id),
	CONSTRAINT FK_fav_books_books FOREIGN KEY(book_id) REFERENCES books(user_id),
	CONSTRAINT PK_fav_books PRIMARY KEY (user_id, book_id)
);

CREATE TABLE IF NOT EXISTS comments (
	id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    book_id INT NOT NULL,
    content VARCHAR(400) NOT NULL,
    fav_count INT DEFAULT 0,
    date_posted DATETIME NOT NULL,
    CONSTRAINT FK_comments_users FOREIGN KEY(user_id) REFERENCES users(user_id),
    CONSTRAINT FK_comments_books FOREIGN KEY(book_id) REFERENCES books(book_id)
);
