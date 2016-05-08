package net.mehmetatas.services.common.impl;

import org.junit.Before;
import org.junit.Test;

import static org.hamcrest.core.Is.is;
import static org.junit.Assert.*;

/**
 * Created by mehmet on 07.05.2016.
 */
public class CryptoProviderImplTest {
    private CryptoProviderImpl crypto;

    @Before
    public void setUp() {
        crypto = new CryptoProviderImpl();
    }

    @Test
    public void crypto_provider_calculates_sha256_hash_for_passwords() {
        // arrange
        String password = "123456";
        String sha256HashOf123456 = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"; // http://www.xorbin.com/tools/sha256-hash-calculator

        // act
        String encryptedPassword = crypto.encryptPassword(password);

        // assert
        assertThat(encryptedPassword, is(sha256HashOf123456));
    }
}