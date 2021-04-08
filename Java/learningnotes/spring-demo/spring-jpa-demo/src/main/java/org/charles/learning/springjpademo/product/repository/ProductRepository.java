package org.charles.learning.springjpademo.product.repository;

import org.charles.learning.springjpademo.product.model.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

/*
 * 产品仓储类。
 */
@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
}
