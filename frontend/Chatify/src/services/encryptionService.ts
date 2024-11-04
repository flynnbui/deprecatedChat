// src/services/encryptionService.js
import * as libsignal from "@privacyresearch/libsignal-protocol-typescript";
import SignalProtocolStore from "./SignalProtocolStore ";

const store = new SignalProtocolStore();

// Function to generate identity keys and registration ID
export async function generateIdentity() {
  const identityKeyPair = await libsignal.KeyHelper.generateIdentityKeyPair();
  const registrationId = libsignal.KeyHelper.generateRegistrationId();

  // Store the keys and registration ID
  store.setIdentityKeyPair(identityKeyPair);
  store.setLocalRegistrationId(registrationId);

  console.log('Identity Key Pair Generated and Stored:', identityKeyPair);
  console.log('Registration ID Generated and Stored:', registrationId);

  return { identityKeyPair, registrationId };
}

// Function to generate pre-keys
export async function generatePreKeys() {
  const preKeyId = 1;
  const signedPreKeyId = 1;

  const preKey = await libsignal.KeyHelper.generatePreKey(preKeyId);
  const signedPreKey = await libsignal.KeyHelper.generateSignedPreKey(
    store.getIdentityKeyPair(),
    signedPreKeyId
  );

  // Store pre-keys
  store.storePreKey(preKeyId, preKey.keyPair);
  store.storeSignedPreKey(signedPreKeyId, signedPreKey.keyPair);

  console.log('PreKey Generated and Stored:', preKey);
  console.log('SignedPreKey Generated and Stored:', signedPreKey);
  return { preKey, signedPreKey };
}

// Function to get the public bundle to send to the server
export function getPublicBundle() {
  const identityKeyPair = store.getIdentityKeyPair();
  const registrationId = store.getLocalRegistrationId();

  const preKey = store.loadPreKey(1);
  const signedPreKey = store.loadSignedPreKey(1);

  return {
    identityKey: arrayBufferToBase64(identityKeyPair.pubKey),
    registrationId: registrationId,
    preKey: {
      keyId: 1,
      publicKey: arrayBufferToBase64(preKey.pubKey),
    },
    signedPreKey: {
      keyId: 1,
      publicKey: arrayBufferToBase64(signedPreKey.pubKey),
      signature: arrayBufferToBase64(signedPreKey.signature),
    },
  };
}

// Helper functions to convert between ArrayBuffer and Base64
export function arrayBufferToBase64(buffer) {
  return btoa(String.fromCharCode(...new Uint8Array(buffer)));
}

export function base64ToArrayBuffer(base64) {
  const binary = atob(base64);
  const len = binary.length;
  const bytes = new Uint8Array(len);
  for (let i = 0; i < len; i++) {
    bytes[i] = binary.charCodeAt(i);
  }
  return bytes.buffer;
}

export { store };
