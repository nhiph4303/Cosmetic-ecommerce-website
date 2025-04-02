# 🛍️ Cosmetic E-Commerce Website

![Project Banner](https://github.com/nhiph4303/cosmetic-ecommerce-website/blob/main/Cosmetic/wwwroot/assets/images/project-banner.png)

A modern and responsive e-commerce platform for selling cosmetics, built with **ASP.NET**, **JavaScript**, **CSS**, and **HTML**.

## 📌 Features
- 🛒 **Product browsing & filtering** – Easily explore a wide range of cosmetics.  
- 🔍 **Search functionality** – Find products by name or category.  
- 🏪 **Shopping cart & checkout** – Add items to the cart and place orders securely.  
- 🔐 **User authentication & authorization** – Secure login and role-based access.  
- 📦 **Order tracking & management** – Users can view and track their orders.  
- 📊 **Admin dashboard** – Manage products, users, and orders efficiently.  

## 🛠 Tech Stack
- **Frontend**: HTML, CSS, JavaScript  
- **Backend**: ASP.NET, C#  
- **Database**: MySQL (via XAMPP)  
- **Version Control**: Git & GitHub  

## 🚀 Getting Started

### Prerequisites
Before running this project, ensure you have the following:
- **XAMPP** installed (for running MySQL)
- **.NET 6+** installed
- **SQL Server** set up (use the database `shop` as specified)
- **Visual Studio 2022+**  

### Installation
1. **Clone the repository**  
   ```sh
   git clone https://github.com/nhiph4303/cosmetic-ecommerce-website.git
   cd cosmetic-ecommerce-website
2. **Set up XAMPP**
- Open XAMPP and start Apache and MySQL.
- Open phpMyAdmin at http://localhost/phpmyadmin.
- Create a new database called shop.
- Import the provided shop.sql file into the shop database via the Import tab in phpMyAdmin.
3. **Configure the Database**
- In the project, open the connection string configuration (usually found in appsettings.json or web.config).
- Update it to connect to your MySQL database (shop)
4. **Run the Project**
- Open the project in Visual Studio.
- Build and run the project using IIS Express or Kestrel.
- Access the website at http://localhost:{port_number}.

## 💻 **For Developers**

### **Setting up the Development Environment**
1. **Install Visual Studio 2022+** and ensure you have the necessary **.NET SDK** and **MySQL** support.

2. **Open the solution file** `Cosmetic.sln` in Visual Studio to work on the project.

### **XAMPP Setup (for MySQL Database)**
1. **Download and install XAMPP** from the [XAMPP website](https://www.apachefriends.org/index.html).

2. **Launch XAMPP Control Panel** and start both **Apache** and **MySQL** services.

3. **Open phpMyAdmin** by navigating to `http://localhost/phpmyadmin`.

4. **Create a database** named `shop` and import the provided `shop.sql` file.

### **Important Files**
- **Cosmetic.sln**: The Visual Studio Solution file.
- **shop.sql**: SQL file containing the database schema for the `shop` database.
- **README.md**: Project documentation.

## 🎯 **Future Improvements**
- **Shopping Cart Functionality**: The shopping cart feature will be enhanced with the ability to update quantities, remove items, and implement real-time updates as items are added or removed.
- **Payment Gateway Integration**: Integrate online payment options such as credit card payments (e.g., PayPal, Stripe) for a complete transaction flow.
- **AI-Powered Q&A**: Implement an AI-powered customer support system to answer frequently asked questions (FAQs) and help customers with real-time queries.

## 📄 **License**
This project is licensed under the **MIT License**.


