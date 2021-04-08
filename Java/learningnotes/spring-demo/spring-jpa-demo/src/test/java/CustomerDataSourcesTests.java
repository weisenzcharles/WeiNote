import org.charles.learning.springjpademo.customer.model.Customer;
import org.charles.learning.springjpademo.customer.repository.CustomerRepository;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.transaction.annotation.Transactional;

import org.junit.Test;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;

@RunWith(SpringRunner.class)
@SpringBootTest
public class CustomerDataSourcesTests {

    @Autowired
    private CustomerRepository customerRepository;

    @Test
    @Transactional("customerTransactionManager")
    public void createCustomer() {

        Customer customer = new Customer("master@weilog.net", "Charles", "Zhang");
        customer = customerRepository.save(customer);
        assertNotNull(customerRepository.findById(customer.getId()));
        assertEquals(customerRepository.findById(customer.getId()).get().getEmail(), "master@weilog.net");
    }
}