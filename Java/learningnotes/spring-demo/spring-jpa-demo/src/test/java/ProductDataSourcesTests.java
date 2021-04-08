import org.charles.learning.springjpademo.product.model.Product;
import org.charles.learning.springjpademo.product.repository.ProductRepository;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.transaction.annotation.Transactional;

import org.junit.Test;

import static org.junit.Assert.assertNotNull;

@RunWith(SpringRunner.class)
@SpringBootTest
public class ProductDataSourcesTests {

    @Autowired
    private ProductRepository productRepository;

    @Test
    @Transactional("productTransactionManager")
    public void createProduct() {
        Product product = new Product("10000", "Book", 80.0);
        product = productRepository.save(product);

        assertNotNull(productRepository.findById(product.getId()));
    }

}