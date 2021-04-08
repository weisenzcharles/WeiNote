package org.charles.learning.springjpademo.customer.repository;

import org.charles.learning.springjpademo.customer.model.Customer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

/*
 * 客户仓储类。
 */
@Repository
public interface CustomerRepository extends JpaRepository<Customer, Integer> {
}
