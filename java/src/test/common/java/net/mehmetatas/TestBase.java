package net.mehmetatas;

import net.mehmetatas.exceptions.MonospadException;
import org.junit.Assert;
import org.junit.Rule;
import org.junit.rules.ExpectedException;
import org.mockito.Mockito;
import org.mockito.verification.VerificationMode;

import java.util.function.Consumer;
import java.util.function.Supplier;

import static org.hamcrest.Matchers.is;
import static org.hamcrest.Matchers.notNullValue;
import static org.mockito.Mockito.times;

/**
 * Created by mehmet on 28.04.2016.
 */
public class TestBase {
    protected static VerificationMode once() {
        return times(1);
    }

    protected static void assertThrows(Class<? extends MonospadException> exceptionClass, Runnable runnable) {
        try {
            runnable.run();
            Assert.fail("No exception was thrown. Expected: " + exceptionClass);
        } catch (Throwable t) {
            if (!t.getClass().equals(exceptionClass)) {
                Assert.fail("Bad exception type. Expected: " + exceptionClass + " Found: " + t.getClass());
            }
        }
    }
}
