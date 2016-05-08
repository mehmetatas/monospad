package net.mehmetatas.services.common.impl;

import net.mehmetatas.services.common.CryptoProvider;
import net.mehmetatas.utils.Utils;
import org.springframework.stereotype.Component;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

/**
 * Created by mehmet on 07.05.2016.
 */
@Component
public class CryptoProviderImpl implements CryptoProvider {
    @Override
    public String encryptPassword(String password) {
        try {
            byte[] bytes = password.getBytes();
            byte[] digestedBytes = MessageDigest.getInstance("SHA-256").digest(bytes);
            return Utils.bytesToHex(digestedBytes);
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        }
        return null;
    }
}
